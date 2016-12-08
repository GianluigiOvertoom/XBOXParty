using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;

namespace CombiDrive
{

    public class Driving : MonoBehaviour
    {
        private enum e_PowerUps
        {
            none,
            Turbo,
            EMP,
            Rocket
        };

        private enum e_Team
        {
            TeamOne,
            TeamTwo 
        };

        //TODO: making an observer pattern with switch points. 
        private GameController m_Controller;

        [SerializeField]
        private e_Team m_CurrentTeam;

        [SerializeField]
        private e_PowerUps m_PowerUp;

        [SerializeField]
        private float m_MoveForce = 180.0f;
        [SerializeField]
        private float m_Drag = 1.0f;

        private int m_CurrentTeamID;

        private int m_CurrentDriverID;

        private Rigidbody m_Rigidbody;

        private List<int> m_TeamMembers = new List<int>(2);

        private bool m_ReversedControlls = false;

        private void Awake()
        {
            m_Controller = GameObject.Find("CombiDriveGameController").GetComponent<GameController>();

            m_Rigidbody = GetComponent<Rigidbody>();
            
            m_Rigidbody.drag = m_Drag;
        }

        private void Start()
        {

            switch (m_CurrentTeam)
            {
                case e_Team.TeamOne:
                    m_TeamMembers = m_Controller.PlayerTeam(0);
                    m_CurrentTeamID = 0;
                    break;
                case e_Team.TeamTwo:
                    m_TeamMembers = m_Controller.PlayerTeam(1);
                    m_CurrentTeamID = 1;
                    break;
                default:
                    m_TeamMembers = m_Controller.PlayerTeam(0);
                    break;
            }
            
            BindButtons(m_ReversedControlls);

            //Bind buttons for powerup usage.
            InputManager.Instance.BindButton("CombiDrive_PowerUpUsage_" + m_TeamMembers[0], m_TeamMembers[0], ControllerButtonCode.RightShoulder, ButtonState.Pressed);
            InputManager.Instance.BindButton("CombiDrive_PowerUpUsage_" + m_TeamMembers[1], m_TeamMembers[1], ControllerButtonCode.RightShoulder, ButtonState.Pressed);
        }
        private void FixedUpdate()
        { 

            //FIXME: HAVE THE ARTIST FIX THIS SHIT, CURRENTLY USING THE RIGHT NEED ROTATION NORMALIZED.
            m_Rigidbody.AddForce(InputManager.Instance.GetAxis("CombiDrive_RightTrigger_" + m_CurrentDriverID) * m_MoveForce * -transform.right);
            m_Rigidbody.AddForce(InputManager.Instance.GetAxis("CombiDrive_LeftTrigger_" + m_CurrentDriverID) * (m_MoveForce / 3) * transform.right);
            //m_Rigidbody.AddForce(InputManager.Instance.GetAxis("CombiDrive_RightTrigger_" + m_CurrentDriverID) * m_MoveForce * transform.forward);
            //m_Rigidbody.AddForce(InputManager.Instance.GetAxis("CombiDrive_LeftTrigger_" + m_CurrentDriverID) * (m_MoveForce / 3) * -transform.forward);
            m_Rigidbody.AddForce((InputManager.Instance.GetButton("CombiDrive_HandBrake_" + m_CurrentDriverID) ? 1 : 0) * (m_MoveForce / 10) * -transform.forward);

            if (InputManager.Instance.GetButton("CombiDrive_PowerUpUsage_" + m_TeamMembers[0]) 
                || InputManager.Instance.GetButton("CombiDrive_PowerUpUsage_" + m_TeamMembers[1]) 
                && m_PowerUp != e_PowerUps.none)
            {
                switch (m_PowerUp)
                {
                    case e_PowerUps.none:
                        break;
                    case e_PowerUps.Turbo:
                        m_MoveForce = 360f;
                        m_Rigidbody.AddForce(m_MoveForce * -transform.right, ForceMode.Impulse);
                        StartCoroutine(TurboCounter());
                        m_PowerUp = e_PowerUps.none;
                        break;
                    case e_PowerUps.EMP:
                        break;
                    case e_PowerUps.Rocket:
                        break;
                }
            }
        }

        private void BindButtons(bool reverseControls)
        {
            if (reverseControls == false)
            {
                //Binding Buttons for driver.
                InputManager.Instance.BindAxis("CombiDrive_RightTrigger_" + m_TeamMembers[0], m_TeamMembers[0], ControllerAxisCode.RightTrigger);
                InputManager.Instance.BindAxis("CombiDrive_LeftTrigger_" + m_TeamMembers[0], m_TeamMembers[0], ControllerAxisCode.LeftTrigger);
                InputManager.Instance.BindButton("CombiDrive_HandBrake_" + m_TeamMembers[0], m_TeamMembers[0], ControllerButtonCode.X, ButtonState.Pressed);
                m_CurrentDriverID = m_TeamMembers[0];
            }
            else
            {
                //Binding Buttons for in reverse driver.
                InputManager.Instance.BindAxis("CombiDrive_RightTrigger_" + m_TeamMembers[1], m_TeamMembers[1], ControllerAxisCode.RightTrigger);
                InputManager.Instance.BindAxis("CombiDrive_LeftTrigger_" + m_TeamMembers[1], m_TeamMembers[1], ControllerAxisCode.LeftTrigger);
                InputManager.Instance.BindButton("CombiDrive_HandBrake_" + m_TeamMembers[1], m_TeamMembers[1], ControllerButtonCode.X, ButtonState.Pressed);
                m_CurrentDriverID = m_TeamMembers[1];
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.name)
            {
                case "SwitchPoint":
                    m_ReversedControlls = !m_ReversedControlls;
                    BindButtons(m_ReversedControlls);
                    m_Controller.SubmitLap(gameObject, m_CurrentTeamID);
                    break;
                case "Turbo":
                    m_PowerUp = e_PowerUps.Turbo;
                    Destroy(other.gameObject);
                    break;
            }
        }

        private IEnumerator TurboCounter()
        {
            yield return new WaitForSeconds(5f);
            m_MoveForce = 180f;
        }
    }
}
