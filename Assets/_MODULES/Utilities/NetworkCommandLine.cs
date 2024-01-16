using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommandLine : MonoSingleton<NetworkCommandLine>
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] bool StartOnAwake = true;

    private void OnEnable()
    {
        if(networkManager == null) networkManager = GetComponent<NetworkManager>();
    }
    private void Start()
    {
        if (networkManager == null) return;

        if (Application.isEditor) return;
        if (StartOnAwake) CheckAndStartNetwork();
    }

    public void CheckAndStartNetwork()
    {
        var args = GetCommandLineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            StartNetWork(mode);
        }
    }
    public void StartNetWork(string mode)
    {
        switch (mode)
        {
            case "server":
                networkManager.StartServer();
                Debug.Log("NETWORK COMMAND LINE: start server");
                break;
            case "host":
                networkManager.StartHost();
                Debug.Log("NETWORK COMMAND LINE: start host");
                break;
            case "client":
                networkManager.StartClient();
                Debug.Log("NETWORK COMMAND LINE: start client");
                break;
        }
    }
    private Dictionary<string, string> GetCommandLineArgs()
    {
        Dictionary<string, string> argTable = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++)
        {
            var arg = args[i].ToLower();
            if(arg.StartsWith("-"))
            {
                var value = i < arg.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;
                argTable.Add(arg, value);

            }
        }
        return argTable;
    }
}
