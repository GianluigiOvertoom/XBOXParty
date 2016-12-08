using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XBOXParty;

namespace CombiDrive
{
    public class UI_Players : MonoBehaviour
    {
        private enum e_Team
        {
            TeamOne,
            TeamTwo
        };

        private GameController m_Controller;

        [SerializeField]
        private e_Team m_CurrentTeam;

        private List<int> m_TeamMembers = new List<int>(2);

        GlobalGameManager instance;

        private Transform m_Player1;
        private Transform m_Player2;

        private Transform m_FlagPlayer1;
        private Transform m_FlagPlayer2;

        private Transform m_NumberPlayer1;
        private Transform m_NumberPlayer2;
        
        private Transform m_WatchOutSwitchedCanvas;



        private void Awake()
        {
            m_Controller = GameObject.Find("CombiDriveGameController").GetComponent<GameController>();

            m_WatchOutSwitchedCanvas = transform.FindChild("Main Camera/SwitchCanvas");
            m_Player1 = transform.FindChild("Player1");
            m_Player2 = transform.FindChild("Player2");

            m_FlagPlayer1 = transform.FindChild("Flag1");
            m_FlagPlayer2 = transform.FindChild("Flag2");

            m_NumberPlayer1 = transform.FindChild("Flag1/Canvas/Text");
            m_NumberPlayer2 = transform.FindChild("Flag2/Canvas/Text");
            

        }

        void Start()
        {
            instance = GlobalGameManager.Instance;

            switch (m_CurrentTeam)
            {
                case e_Team.TeamOne:
                    m_TeamMembers = m_Controller.PlayerTeam(0);
                    break;
                case e_Team.TeamTwo:
                    m_TeamMembers = m_Controller.PlayerTeam(1);
                    break;
                default:
                    m_TeamMembers = m_Controller.PlayerTeam(0);
                    break;
            }

            //Set Colours player1 and number.
            m_Player1.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[0]));
            m_Player1.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[0]));

            m_FlagPlayer1.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[0]));
            m_FlagPlayer1.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[0]));

            m_NumberPlayer1.GetComponent<Text>().text =  (1 + m_TeamMembers[0]).ToString();

            //Set colours player2 and number.
            m_Player2.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[1]));
            m_Player2.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[1]));

            m_FlagPlayer2.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[1]));
            m_FlagPlayer2.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[1]));

            m_NumberPlayer2.GetComponent<Text>().text = (1 + m_TeamMembers[1]).ToString();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "SwitchPoint")
            {

                m_WatchOutSwitchedCanvas.gameObject.SetActive(true);
                StartCoroutine(ShowSwitchImage());

                //Set colours player1 and number.
                m_Player1.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[1]));
                m_Player1.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[1]));

                m_FlagPlayer1.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[1]));
                m_FlagPlayer1.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[1]));

                m_NumberPlayer1.GetComponent<Text>().text = (1 + m_TeamMembers[1]).ToString();

                //Set colours player2 and number.
                m_Player2.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[0]));
                m_Player2.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[0]));

                m_FlagPlayer2.GetComponent<Renderer>().material.SetColor("_EmissionColor", (Color)instance.GetPlayerColor(m_TeamMembers[0]));
                m_FlagPlayer2.GetComponent<Renderer>().material.SetColor("_Color", (Color)instance.GetPlayerColor(m_TeamMembers[0]));

                m_NumberPlayer2.GetComponent<Text>().text = (1 + m_TeamMembers[0]).ToString();


            }
        }

        private IEnumerator ShowSwitchImage()
        {

            yield return new WaitForSeconds(2f);
            m_WatchOutSwitchedCanvas.gameObject.SetActive(false);
        }
    }
}