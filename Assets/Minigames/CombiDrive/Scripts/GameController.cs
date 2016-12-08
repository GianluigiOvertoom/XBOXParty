using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using XBOXParty;

namespace CombiDrive
{
    public class GameController : MonoBehaviour
    {
        GlobalGameManager instance;
        private int[] m_Players = new int[4];
        List<int> m_SubmitList;

        private List<int> m_TeamOne = new List<int>(2);
        private List<int> m_TeamTwo = new List<int>(2);

        private List<int> m_TeamOneWon = new List<int>();
        private List<int> m_TeamTwoWon = new List<int>();


        private float m_LapTimeTeamOne;
        private float m_LapTimeTeamTwo;

        private int m_LapsTeamOne;
        private int m_LapsTeamTwo;

        private int m_WinningTeam;



        private void Awake()
        {

            //Get the instance of the GlobalGameMananger.
            instance = GlobalGameManager.Instance;
            
            //Get the teamID for each player.
            for (int i = 0; i < m_Players.Length; i++)
            {
                m_Players[i] = instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2);

                if (m_Players[i] == 0)
                {
                    m_TeamOne.Add(i);
                }
                else
                {
                    m_TeamTwo.Add(i);
                }

            }

            //Set lists for team winning.
            m_TeamOneWon.Add(m_Players[0]);
            m_TeamOneWon.Add(m_Players[1]);
            m_TeamOneWon.Add(m_Players[2]);
            m_TeamOneWon.Add(m_Players[3]);

            m_TeamTwoWon = SetWinningTeam(m_TeamOneWon);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("RaceTrackScene");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }

            m_LapTimeTeamOne += Time.deltaTime;
            m_LapTimeTeamTwo += Time.deltaTime;

        }

        public List<int> PlayerTeam(int team)
        {
            if (team == 0)
            {
                return m_TeamOne;

            }
            else
            {
                return m_TeamTwo;

            }
        }

        public void SubmitLap(GameObject car, int team)
        {
            switch (team)
            {
                case 1:
                    if (m_LapTimeTeamOne >= 60.0f)
                    {
                        m_LapsTeamOne += 1;
                        m_LapTimeTeamOne = 0.0f;
                    }
                    break;

                case 2:
                    if (m_LapTimeTeamTwo >= 60.0f)
                    {
                        m_LapsTeamTwo += 1;
                        m_LapTimeTeamTwo = 0.0f;
                    }
                    break;
            }

            if (m_LapsTeamOne >= 2)
            {
                instance.SubmitGameResults(m_TeamOneWon);
            }
            if (m_LapsTeamTwo >= 2)
            {
                instance.SubmitGameResults(m_TeamTwoWon);
            }
        }

        public List<int> SetWinningTeam(List<int> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == 0)
                {
                    players[i] = 2;
                }
                else
                {
                    players[i] = 0;
                }

            }
            return players;
        } 
    }
}
