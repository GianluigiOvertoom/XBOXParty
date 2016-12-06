using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace XBOXParty
{
    public enum MinigameMode
    {
        MODE_FFA = 0,
        MODE_2V2 = 1,
        MODE_1V3 = 2
    }

    public class MinigameManager : MonoBehaviour
    {
        [SerializeField]
        private List<string> _minigamesFFA;

        [SerializeField]
        private List<string> _minigames2v2;

        [SerializeField]
        private List<string> _minigames1v3;

        [SerializeField]
        private string _debugMinigame;

        private void Awake()
        {
            GlobalGameManager.Instance.MiniGameStartEvent += OnStartMinigame;
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
                GlobalGameManager.Instance.MiniGameStartEvent -= OnStartMinigame;
        }

        private void OnStartMinigame()
        {
            if (GlobalGameManager.Instance.GameState != GameState.STATE_BOARD)
                return;

            Debug.Log("Starting minigame!");

            if (_debugMinigame != "")
            {
                GlobalGameManager.Instance.SetCurrentMinigameID(-1);
                SceneManager.LoadScene(_debugMinigame);
                return;
            }

            //Determine random gamemode
            int gameMode = 0;
            string levelName = "";

            //Only play 2v2 & 1v3 if it's a 4 player game.
            if (GlobalGameManager.Instance.PlayerCount >= 4)
            {
                gameMode = Random.Range(0, 99);
                gameMode /= 3;
            }

            //Determine the level
            int minigameID = 0;

            switch ((MinigameMode)gameMode)
            {
                case MinigameMode.MODE_FFA:
                {
                    minigameID = Random.Range(0, _minigamesFFA.Count);
                    levelName = _minigamesFFA[minigameID];
                    break;
                }

                case MinigameMode.MODE_2V2:
                {
                    minigameID = Random.Range(0, _minigames2v2.Count);
                    levelName = _minigamesFFA[minigameID];
                    break;
                }

                case MinigameMode.MODE_1V3:
                {
                    minigameID = Random.Range(0, _minigames1v3.Count);
                    levelName = _minigamesFFA[minigameID];
                    break;
                }

                default:
                    break;
            }

            GlobalGameManager.Instance.SetCurrentMinigameID(minigameID);
            SceneManager.LoadScene(levelName);
        }
    }
}