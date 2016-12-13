using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XBOXParty;

namespace Board
{
    public class MinigameSelectionMenu : MonoBehaviour
    {
        [SerializeField]
        private MinigameManager m_MinigameManager;
        private List<MinigameData> m_Minigames;
        private int m_CurrentMinigameID;

        [SerializeField]
        private MinigameSelectionIcon[] m_SelectionIcons;

        private void Awake()
        {
            m_Minigames = new List<MinigameData>();
        }

        private void Start()
        {
            GlobalGameManager.Instance.MiniGameStartEvent += OnStartMinigame;
            GlobalGameManager.Instance.GameStartEvent += OnGameStart;
            GlobalGameManager.Instance.ResetGameEvent += OnResetGame;

            //Bind buttons
            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.BindButton("Board_StartStopWheel_" + i, i, ControllerButtonCode.A, ButtonState.OnPress);
                InputManager.Instance.BindButton("Board_RerollWheel_" + i, i, ControllerButtonCode.Y, ButtonState.OnPress);
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.MiniGameStartEvent -= OnStartMinigame;
                GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
                GlobalGameManager.Instance.ResetGameEvent -= OnResetGame;
            }

            for (int i = 0; i < 4; ++i)
            {
                InputManager.Instance.UnbindButton("Board_StartStopWheel_" + i);
                InputManager.Instance.UnbindButton("Board_RerollWheel_" + i);
            }
        }

        private void Update()
        {
            for (int i = 0; i < 4; ++i)
            {
                for (int iconID = 0; iconID < m_SelectionIcons.Length; ++iconID)
                {


                    if (InputManager.Instance.GetButton("Board_StartStopWheel_" + i))
                    {
                        //Start rolling
                        //if (m_SelectionIcons[iconID].IsRolling == false && m_SelectionIcons[iconID].IsStopping == false) { }

                        //Stop rolling
                        if (m_SelectionIcons[iconID].IsRolling == true &&
                            m_SelectionIcons[iconID].IsStopping == false)
                        {
                            m_SelectionIcons[iconID].StopRolling();
                        }

                        //Start playing!
                        if (m_SelectionIcons[iconID].IsRolling == false &&
                            m_SelectionIcons[iconID].IsStopping == true)
                        {
                            if (m_SelectionIcons[iconID].IsChosen)
                            {
                                m_MinigameManager.LoadMinigame(m_SelectionIcons[iconID].GetMinigameData());
                            }    
                        }
                    }


                    if (InputManager.Instance.GetButton("Board_RerollWheel_" + i))
                    {
                        //Reroll
                        if (m_SelectionIcons[iconID].IsRolling == false &&
                            m_SelectionIcons[iconID].IsStopping == true)
                        {
                            //Remove this minigame from the list
                            if (m_SelectionIcons[iconID].IsChosen)
                            {
                                if (m_Minigames.Count > 1)
                                    m_Minigames.Remove(m_SelectionIcons[iconID].GetMinigameData());
                            }

                            //Roll again!
                            m_SelectionIcons[iconID].StartRolling();
                        }
                    }
                }
            }
        }

        private void OnStartMinigame()
        {
            //Copy the list
            m_CurrentMinigameID = 0;

            m_Minigames.Clear();
            foreach (MinigameData minigame in m_MinigameManager.GenerateMinigameList())
            {
                m_Minigames.Add(minigame);
            }

            //Start rolling
            for (int iconID = 0; iconID < m_SelectionIcons.Length; ++iconID)
            {
                m_SelectionIcons[iconID].SetMinigameData(GetNextMinigame());
                m_SelectionIcons[iconID].StartRolling();
            }

            Show();
        }

        private void OnGameStart()
        {
            Hide();
        }

        private void OnResetGame()
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public MinigameData GetNextMinigame()
        {
            m_CurrentMinigameID += 1;

            if (m_CurrentMinigameID >= m_Minigames.Count)
                m_CurrentMinigameID = 0;

            return m_Minigames[m_CurrentMinigameID];
        }
    }
}
