using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace XBOXParty
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField]
        private List<MinigameData> m_Minigames;

        [SerializeField]
        private string m_DebugMinigame;

        private void Awake()
        {
            GlobalGameManager.Instance.MiniGameStartEvent += OnStartMinigame;
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
                GlobalGameManager.Instance.MiniGameStartEvent -= OnStartMinigame;
        }

        //Only for debug purposes
        private void OnStartMinigame()
        {
            if (GlobalGameManager.Instance.GameState != GameState.STATE_BOARD)
                return;

            Debug.Log("Starting minigame!");

            if (m_DebugMinigame != "")
            {
                GlobalGameManager.Instance.SetCurrentMinigame(null);
                SceneManager.LoadScene(m_DebugMinigame);
                return;
            }
        }

        public List<MinigameData> GenerateMinigameList()
        {
            List<MinigameData> playableMinigames = new List<MinigameData>();

            foreach(MinigameData minigame in m_Minigames)
            {
                //Never play the same game twice
                if (GlobalGameManager.Instance.GetCurrentMinigame() != minigame)
                {
                    //Always be able to play FFA
                    if (minigame.GameMode == MinigameMode.MODE_FFA)
                    {
                        playableMinigames.Add(minigame);
                    }

                    //Only play 2v2 & 1v3 if it's a 4 player game.
                    else
                    {
                        if (GlobalGameManager.Instance.PlayerCount >= 4)
                        {
                            playableMinigames.Add(minigame);
                        }
                    }
                }
            }

            return playableMinigames;
        }

        public void LoadMinigame(MinigameData minigame)
        {
            GlobalGameManager.Instance.SetCurrentMinigame(minigame);
            SceneManager.LoadScene(minigame.RootScene);
        }
    }
}