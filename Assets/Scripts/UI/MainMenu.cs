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

            //Bind button
            InputManager.Instance.BindButton("Board_StartGame", 0, ControllerButtonCode.Start, ButtonState.OnPress);
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
                GlobalGameManager.Instance.ResetGameEvent -= OnResetGame;
            }

            InputManager.Instance.UnbindButton("Board_StartGame");
        }

        private void Update()
        {
            if (GlobalGameManager.Instance.PlayerCount < 2)
                return;

            if (InputManager.Instance.GetButton("Board_StartGame"))
            {
                StartGame();
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
