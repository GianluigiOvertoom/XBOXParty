using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;

namespace SquareSiege
{
    public class TeamController : MonoBehaviour
    {
        GlobalGameManager m_GlobalGameManager;

        [SerializeField]
        private MeshRenderer[] m_P1Flags, m_P2Flags;

        private GameObject m_Tank, m_Marker;

        private GameController m_GameController;

        private TankController m_TankController;
        public TankController Tank { get { return m_TankController; } }

        private MarkerController m_MarkerController;
        public MarkerController Marker { get { return m_MarkerController; } }

        private string m_NoFlagError = "Each player must have a flag!";

        private List<int> m_Players = new List<int>(2);
        public List<int> Players
        {
            get { return m_Players; }
        }

        [SerializeField]
        private int m_MaxLives = 3;
        public int MaxLives { get { return m_MaxLives; } }

        [SerializeField]
        private int m_Lives = 0;
        public int Lives { get { return m_Lives; } }

        void Awake()
        {
            m_GlobalGameManager = GlobalGameManager.Instance;

            m_GameController = FindObjectOfType<GameController>();

            m_Tank = transform.GetChild(0).gameObject;
            m_Marker = transform.GetChild(1).gameObject;

            m_TankController = m_Tank.GetComponent<TankController>();
            m_MarkerController = m_Marker.GetComponent<MarkerController>();
        }

        // Use this for initialization
        void Start()
        {
            // Assign the players to the tank controls
            m_TankController.SetPlayerPositions(m_Players[0], m_Players[1]);

            SetFlagColours();

            m_Lives = m_MaxLives;
        }

        private void SetFlagColours()
        {
            if (m_P1Flags.Length > 0 && m_P2Flags.Length > 0)
            {
                foreach (MeshRenderer mr in m_P1Flags)
                {
                    mr.material.color = m_GlobalGameManager.GetPlayerColor(m_Players[0]);
                }

                foreach (MeshRenderer mr in m_P2Flags)
                {
                    mr.material.color = m_GlobalGameManager.GetPlayerColor(m_Players[1]);
                }
            }
            else Debug.LogError(m_NoFlagError);
        }

        public void AssignPlayer( int playerID )
        {
            m_Players.Add(playerID);
        }

        public void InflictDamage( int amount )
        {
            if (m_TankController.IsAlive)
            {
                m_Lives -= amount;
                m_MarkerController.RemoveHealth();

                if (m_Lives < 1) m_TankController.Die();

                float vib = 3f;

                ControllerInput.SetVibration(m_TankController.BasePlayer, vib, vib, .5f);
                ControllerInput.SetVibration(m_TankController.TurretPlayer, vib, vib, .5f);
            }
        }

        public void Lose()
        {
            if (transform.name == m_GameController.Team1.name) m_GameController.Win(PlayerTeam.Team2);
            else if (transform.name == m_GameController.Team2.name) m_GameController.Win(PlayerTeam.Team1);
        }
    }
}