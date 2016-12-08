using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;

namespace CombiDrive
{
    public class Steering : MonoBehaviour
    {
        private enum e_Team
        {
            TeamOne,
            TeamTwo
        };

        private GameController m_Controller;

        [SerializeField]
        private e_Team m_CurrentTeam;

        [SerializeField]
        private float m_RotateTorque = 12.0f;
        [SerializeField]
        private float m_AngularDrag = 0.5f;

        private int m_CurrentDriverID;

        private Rigidbody m_Rigidbody;

        private List<int> m_TeamMembers = new List<int>(2);

        private bool m_ReversedControlls = false;

        private void Awake()
        {
            m_Controller = GameObject.Find("CombiDriveGameController").GetComponent<GameController>();

            m_Rigidbody = GetComponent<Rigidbody>();

            m_Rigidbody.angularDrag = m_AngularDrag;
        }

        private void Start()
        {

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
            BindButtons(false);
        }


        private void LateUpdate()
        {
            m_Rigidbody.AddTorque(InputManager.Instance.GetAxis("CombiDrive_LeftThumbStickX_" + m_CurrentDriverID) * m_RotateTorque * Vector3.up);
        }

        private void BindButtons(bool reverseControls)
        {
            if (reverseControls == false)
            {
                //Binding Buttons for Steering.
                InputManager.Instance.BindAxis("CombiDrive_LeftThumbStickX_" + m_TeamMembers[1], m_TeamMembers[1], ControllerAxisCode.LeftStickX);
              
                m_CurrentDriverID = m_TeamMembers[1];
            }
            else
            {
                //Binding Buttons for in reverse for Steering.
                InputManager.Instance.BindAxis("CombiDrive_LeftThumbStickX_" + m_TeamMembers[0], m_TeamMembers[0], ControllerAxisCode.LeftStickX);
                m_CurrentDriverID = m_TeamMembers[0];
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "SwitchPoint")
            {
                m_ReversedControlls = !m_ReversedControlls;
                BindButtons(m_ReversedControlls);
            }
        }
    }
}