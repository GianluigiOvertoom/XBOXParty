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

            List<MinigameData> playableMinigames = new List<MinigameData>();

            foreach(MinigameData data in m_Minigames)
            {
                //Never play the same game twice
                if (GlobalGameManager.Instance.GetCurrentMinigame() != data)
                {
                    //Always be able to play FFA
                    if (data.GameMode == MinigameMode.MODE_FFA)
                    {
                        playableMinigames.Add(data);
                    }

                    //Only play 2v2 & 1v3 if it's a 4 player game.
                    else
                    {
                        if (GlobalGameManager.Instance.PlayerCount >= 4)
                        {
                            playableMinigames.Add(data);
                        }
                    }
                }
            }

            int randomID = Random.Range(0, playableMinigames.Count);
            MinigameData currenteMinigame = playableMinigames[randomID]; 

            GlobalGameManager.Instance.SetCurrentMinigame(currenteMinigame);
            SceneManager.LoadScene(currenteMinigame.RootScene);
        }
    }
}