using UnityEngine;
using System.Collections;
using XBOXParty;

namespace Board
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pressStart;

        private void Start()
        {
            GlobalGameManager.Instance.GameStartEvent += OnGameStart;
            GlobalGameManager.Instance.ResetGameEvent += OnResetGame;

            if (GlobalGameManager.Instance.GameState != GameState.STATE_MAINMENU)
            {
                Hide();
            }

            //Bind buttons
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.BindButton("Board_StartGame_" + i, i, ControllerButtonCode.Start, ButtonState.OnPress);
            }
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
                GlobalGameManager.Instance.ResetGameEvent -= OnResetGame;
            }

            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.UnbindButton("Board_StartGame_" + i);
            }
        }

        private void Update()
        {
            if (GlobalGameManager.Instance.PlayerCount < 2)
                return;

            for (int i = 0; i < 4; ++i)
            {
                if (InputManager.Instance.GetButton("Board_StartGame_" + i))
                {
                    StartGame();
                    return;
                }
            }
        }

        private void OnGameStart()
        {
            Hide();
        }

        private void OnResetGame()
        {
            Show();
            _pressStart.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        //Functions for the UI elements
        public void SetPlayerCount(int playerCount)
        {
            GlobalGameManager.Instance.SetPlayerCount(playerCount);

            if (_pressStart != null)
                _pressStart.SetActive(true);
        }

        public void StartGame()
        {
            GlobalGameManager.Instance.StartGame();
        }
    }
}
