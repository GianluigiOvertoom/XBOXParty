using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using RoboRush;
using XBOXParty;

namespace RoboRush
{
    public class PlayerController : MonoBehaviour
    {
        public UnityEvent m_OnGameFinal;
        public event VoidDelegate m_OnGameFinished;

        private PlayerInput[] m_team1 = new PlayerInput[2];
        private PlayerInput[] m_team2 = new PlayerInput[2];

        private List<int> m_winners;
        private List<int> m_steps;

        [SerializeField]
        private ButtonUI m_textTeam1A;
        [SerializeField]
        private ButtonUI m_textTeam1B;
        [SerializeField]
        private ButtonUI m_textTeam2A;
        [SerializeField]
        private ButtonUI m_textTeam2B;

        [SerializeField]
        private PlayerSprite m_Endscreen;

        private bool m_IsInputActive;

        [Space(4f)]
        [SerializeField]
        Robot m_RobotTeam1;
        [SerializeField]
        Robot m_RobotTeam2;

        private void Awake()
        {
            if (!GlobalGameManager.Instance)
            {
                Debug.LogError(name + ": GlobalGameManager not found, have you tried starting it through the board game?");
                return;
            }
            InitInput();
            BindControllers();
        }

        public void Quit()
        {
            GlobalGameManager.Instance.SubmitGameResults(m_steps);
            Debug.Log("Roborush: Submitted Winners.");
        }

        private void Start()
        {
            BindEndToRobot(m_RobotTeam1);
            BindEndToRobot(m_RobotTeam2);
        }

        public void End(int TeamNumber)
        {
            SetWinners(TeamNumber);
            Debug.Log(m_winners.Count);
            m_Endscreen.SetPlayerSprites(m_winners);
            Deactviate();
            m_OnGameFinal.Invoke();
            m_OnGameFinished.Invoke();
        }

        private void BindEndToRobot(Robot robot)
        {
            robot.UponReachingFinish += End;
        }

        public void SetWinners(int team)
        {
            if (!GlobalGameManager.Instance)
            {
                Debug.Log("Finish without Gamemanager Detected, using 0,1,2,3 as winners");
                this.m_winners = new List<int> { 0,1,2,3};
                return;
            }

            Debug.Log("Roborush: Recieving Winners");

            List<int> winners = new List<int>();
            List<int> steps = new List<int>();
            if (team == 0) // Blue
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 0)
                        winners.Add(i);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 1)
                        winners.Add(i);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 1)
                        steps.Add(2);
                    else steps.Add(0);
                }
            }

            if (team == 1) // Red
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 1)
                        winners.Add(i);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 0)
                        winners.Add(i);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 0)
                        steps.Add(2);
                    else steps.Add(0);
                }
            }

            m_winners = winners;
            m_steps = steps;
            Debug.Log("Roborush: Winners are: " + winners );
        }

        public void Activate()
        {
            m_IsInputActive = true;
        }

        public void Deactviate()
        {
            m_IsInputActive = false;
        }

        public void InitInput()
        {
            // Cheap lazy way to sort players per team.
            for (int i = 0; i < 4; i++)
            {
                if (GlobalGameManager.Instance.GetPlayerTeamID(i, MinigameMode.MODE_2V2) == 0)
                {
                    if (m_team1[0] == null)
                        m_team1[0] = new PlayerInput(i);
                    else m_team1[1] = new PlayerInput(i);
                }
                else
                {
                    if (m_team2[0] == null)
                        m_team2[0] = new PlayerInput(i);
                    else m_team2[1] = new PlayerInput(i);
                }
            }
        }

        void BindControllers()
        {
            if (m_textTeam1A)
            {
                m_team1[0].OnButtonChange += m_textTeam1A.ChangeText;
                m_team1[0].OnButtonPressed += m_RobotTeam1.MoveLeft;
                m_textTeam1A.SetPlayer(m_team1[0].playerNumber, GlobalGameManager.Instance.GetPlayerColor(m_team1[0].playerNumber));
                m_OnGameFinished += m_textTeam1A.Close;
            }
            if (m_textTeam1B)
            {
                m_team1[1].OnButtonChange += m_textTeam1B.ChangeText;
                m_team1[1].OnButtonPressed += m_RobotTeam1.MoveRight;
                m_textTeam1B.SetPlayer(m_team1[1].playerNumber, GlobalGameManager.Instance.GetPlayerColor(m_team1[1].playerNumber));
                m_OnGameFinished += m_textTeam1B.Close;
            }
            if (m_textTeam2A)
            {
                m_team2[0].OnButtonChange += m_textTeam2A.ChangeText;
                m_team2[0].OnButtonPressed += m_RobotTeam2.MoveLeft;
                m_textTeam2A.SetPlayer(m_team2[0].playerNumber, GlobalGameManager.Instance.GetPlayerColor(m_team2[0].playerNumber));
                m_OnGameFinished += m_textTeam2A.Close;
            }
            if (m_textTeam2B)
            {
                m_team2[1].OnButtonChange += m_textTeam2B.ChangeText;
                m_team2[1].OnButtonPressed += m_RobotTeam2.MoveRight;
                m_textTeam2B.SetPlayer(m_team2[1].playerNumber, GlobalGameManager.Instance.GetPlayerColor(m_team2[1].playerNumber));
                m_OnGameFinished += m_textTeam2B.Close;
            }
        }

        public void ForceRandombuttons()
        {
            if (InputManager.Instance)
            {
                m_team1[0].ForceButtonChange();
                m_team1[1].ForceButtonChange();
                m_team2[0].ForceButtonChange();
                m_team2[1].ForceButtonChange();
            }
        }

        private void Update()
        {
            CheckButtons();
        }

        private void CheckButtons()
        {
            if (m_IsInputActive)
            {
                if (m_team1[0] != null) m_team1[0].CheckButton();
                if (m_team1[1] != null) m_team1[1].CheckButton();
                if (m_team2[0] != null) m_team2[0].CheckButton();
                if (m_team2[1] != null) m_team2[1].CheckButton();

                if (Input.GetKeyDown(KeyCode.A))
                {
                    m_RobotTeam1.MoveLeft();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    m_RobotTeam1.MoveRight();
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    m_RobotTeam2.MoveLeft();
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    m_RobotTeam2.MoveRight();
                }
            }
        }
    }
}