using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class TitleController : MonoSingleton<TitleController>
    {
        public enum TitleState
        {
            NONE = -1,
            INIT = 0,
            READY = 1
        }
        public enum NetworkGameType
        {
            NONE,
            HOST,
            SERVER,
            CLIENT
        }
        [SerializeField] private Button btnStartGame;
        [SerializeField] private string nextSceneEnter;
        [SerializeField] private NetworkGameType networkToJoinAs = NetworkGameType.HOST;
        [SerializeField] TitleState _currentState;

        private void Awake()
        {
            if(btnStartGame == null) btnStartGame = GetComponentInChildren<Button>();
        }

        private void Start()
        {
            _currentState = TitleState.INIT;
        }
        private void OnEnable()
        {
            if(btnStartGame) btnStartGame.onClick.AddListener(OnClickBtnStartGame);
        }
        private void OnDisable()
        {
            if (btnStartGame) btnStartGame.onClick.RemoveListener(OnClickBtnStartGame);
        }

        private void OnClickBtnStartGame()
        {
            switch ((TitleState)_currentState)
            {
                case TitleState.INIT:
                    _currentState++;
                    break;
                case TitleState.READY:
                    NetworkCommandLine.instance.StartNetWork(networkToJoinAs.ToString().ToLower());
                    LoadingScreenManager.instance.LoadScene(nextSceneEnter);
                    _currentState = TitleState.INIT;
                    break;
            }
        }

    }
}


