using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;
using UnityEngine.SceneManagement;

namespace SquareSiege
{
    public delegate void PlayerCountError();

    public enum PlayerTeam
    {
        Team1 = 0,
        Team2
    }

    public class GameController : MonoBehaviour
    {
        #region Error Events
        private event PlayerCountError m_PlayerCountError;
        public PlayerCountError PlayerCountErrorEvent
        {
            get { return m_PlayerCountError; }
            set { m_PlayerCountError = value; }
        }

        private string m_PlayerCountErrorMessage = "There must be 4 players!";
        #endregion

        GlobalGameManager m_GlobalGameManager;

        [SerializeField]
        private bool m_Dev = false; // DONT FORGET TO SET TO FALSE!!!!!!!!
        public bool Dev { get { return m_Dev; } }

        private int m_RequiredPlayers = 4;

        [SerializeField]
        private TeamController m_Team1, m_Team2;
        public TeamController Team1 { get { return m_Team1; } }
        public TeamController Team2 { get { return m_Team2; } }

        [SerializeField]
        private List<GameObject> m_Spawnpoints;
        public List<GameObject> Spawnpoints { get { return m_Spawnpoints; } }

        [SerializeField]
        private List<AudioClip> m_MusicTracks;

        private SoundController m_SoundController;

        void Awake()
        {
            m_GlobalGameManager = GlobalGameManager.Instance;

            m_SoundController = GetComponent<SoundController>();

            RegisterInput();

            //if (m_Team1 == null) m_Team1 = GameObject.Find("Team1").GetComponent<TeamController>();
            //if (m_Team2 == null) m_Team2 = GameObject.Find("Team2").GetComponent<TeamController>();
        }

        // Use this for initialization
        void Start()
        {
            AssignPlayers();

            SetTankPositions();

            PlayMusicTracks();
        }

        private void PlayMusicTracks()
        {
            foreach (AudioClip ac in m_MusicTracks)
            {
                m_SoundController.PlaySound(ac, true);
            }
        }

        private void SetTankPositions()
        {
            Vector3 pos1 = Vector3.zero;
            Vector3 pos2 = Vector3.zero;

            pos1 = m_Spawnpoints[Random.Range(0, m_Spawnpoints.Count)].transform.position;

            do pos2 = m_Spawnpoints[Random.Range(0, m_Spawnpoints.Count)].transform.position;
            while (pos2 == pos1);

            m_Team1.Tank.transform.position = pos1;
            m_Team2.Tank.transform.position = pos2;
        }

        public void Win( PlayerTeam team )
        {
            List<int> positions = new List<int>();

            // Set all to 2
            for (int i = 0; i < m_GlobalGameManager.PlayerCount; i++)
            {
                positions.Add(2);
            }

            // Set winning player positions to 0
            if (team == PlayerTeam.Team1)
            {
                positions[m_Team1.Players[0]] = 0;
                positions[m_Team1.Players[1]] = 0;
            }
            else
            {
                positions[m_Team2.Players[0]] = 0;
                positions[m_Team2.Players[1]] = 0;
            }

            m_GlobalGameManager.SubmitGameResults(positions);
        }

        private void RegisterInput()
        {
            for (int i = 0; i < m_GlobalGameManager.PlayerCount; i++)
            {
                InputManager.Instance.BindAxis("SquareSiege_LH" + i, i, ControllerAxisCode.LeftStickX);
                InputManager.Instance.BindAxis("SquareSiege_LV" + i, i, ControllerAxisCode.LeftStickY);
                InputManager.Instance.BindAxis("SquareSiege_RH" + i, i, ControllerAxisCode.RightStickX);
                InputManager.Instance.BindAxis("SquareSiege_RV" + i, i, ControllerAxisCode.RightStickY);
                InputManager.Instance.BindButton("SquareSiege_Shoot" + i, i, ControllerButtonCode.A, ButtonState.OnPress);
            }
        }

        private void AssignPlayers()
        {
            if (m_GlobalGameManager.PlayerCount == m_RequiredPlayers || m_Dev)
            {
                m_Team1.AssignPlayer(0);
                m_Team1.AssignPlayer(3);
                m_Team2.AssignPlayer(1);
                m_Team2.AssignPlayer(2);
            }
            else
            {
                //FIXME: Dont log error, but prompt in-game message or something.
                Debug.LogError(m_PlayerCountErrorMessage);
                if (m_PlayerCountError != null) m_PlayerCountError();
            }
        }
    }
}
