using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TD.Character
{
    public class PlayerUIManager : MonoSingleton<PlayerUIManager>
    {
        [Header("NETWORK JOIN-TEST")]
        [SerializeField] private bool startAsClient;
        public event Action OnNetworkJoin;
        private bool _isPlayerJoinAsClient;
        private void Awake()
        {
            _isPlayerJoinAsClient = startAsClient;
        }
        private void OnEnable()
        {
            OnNetworkJoin += OnPlayerJoinGame;
        }
        private void OnDisable()
        {
            OnNetworkJoin -= OnPlayerJoinGame;
        }
        private void OnPlayerJoinGame()
        {
            startAsClient = !startAsClient;
            _isPlayerJoinAsClient = startAsClient;

            NetworkManager.Singleton.Shutdown();
            if (startAsClient)
            {
                NetworkManager.Singleton.StartClient();
            }else
            {
                NetworkManager.Singleton.StartHost();
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnNetworkJoin?.Invoke();
            }
#endif
        }
    }
}

