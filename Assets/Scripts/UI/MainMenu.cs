using UnityEngine;
using System.Collections;

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
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
                GlobalGameManager.Instance.ResetGameEvent -= OnResetGame;
            }
        }

        private void OnGameStart()
        {
            Hide();
        }

        private void OnResetGame()
        {
            Show();
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
