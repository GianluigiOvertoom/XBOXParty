using SerializableAttribute = System.SerializableAttribute;
using UnityEngine;
using System.Collections.Generic;
using XBOXParty;
namespace RoboRush
{
    [Serializable]
    public class RobotMovement
    {
        public event VoidDelegate OnMoveEnd;

        private Transform m_transform;
        private RobotAction m_previousAction;
        private RobotAction m_currentAction;
        private int m_currentStep;
        private Vector3[] m_steps;
        private bool[] m_stepPossible;
        private float m_delay;
        private bool m_playerIsMoving = true;

        public bool nextMovePossible
        {
            get
            {
                return m_stepPossible[m_currentStep];
            }
        }

        public bool isOnLastMove
        {
            get
            {
                return (m_currentStep == m_steps.Length-1);
            }
        }

        public void performAction(RobotAction action)
        {
            if (isOnLastMove)
            {
                Debug.Log("Robot is already at End");
                return;
            }
            m_playerIsMoving = true;
            m_currentAction = action;

            if (action.actionType == RobotActionType.StaggerLeft||action.actionType == RobotActionType.StaggerRight)
            {
                m_playerIsMoving = false;
                return;
            }

            if (m_stepPossible[m_currentStep])
            {
                // when the step is possible..
                if (action.actionType != RobotActionType.Jump)
                {
                    m_currentStep += 1;
                }
                else m_playerIsMoving = false;
            }
            else
            {
                // when the step is impossible..
                if (action.actionType == RobotActionType.Jump)
                {
                    m_currentStep += 1;
                }
                else m_playerIsMoving = false;
            }
        }

        public void PrecalculatePath(Vector3[] pathNodes)
        {
            // To save positions and if we can walk to the next node.
            List<Vector3> positions = new List<Vector3>();
            List<bool> isMoveValid = new List<bool>();

            // Calculate if each position is possible. if it isn't, read out
            // block normal movement and force players to jump.
            for (int i = 0; i < pathNodes.Length - 1; i++)
            {
                RaycastHit hit;
                Ray ray = new Ray(pathNodes[i],Vector3.right);

                if (Physics.Raycast(ray, out hit, Vector3.Distance(pathNodes[i], pathNodes[i + 1])))
                {
                    isMoveValid.Add(false);
                    positions.Add(pathNodes[i]);
                    i += hit.collider.gameObject.GetComponent<Obstacle>().StepsToJump;
                }
                else
                {
                    isMoveValid.Add(true);
                    positions.Add(pathNodes[i]);
                }
            }

            // Save them as arrays, we're not changing this
            // anymore.
            m_steps = positions.ToArray();
            m_stepPossible = isMoveValid.ToArray();
        }

        public void OnGizmoDraw()
        {
            // Draw each position in the list
            // red or blue depending if we can
            // walk or are forced to jump over
            // an obstacle.
            if (m_steps != null)
            {
                for (int i = 0; i < m_steps.Length; i++)
                {
                    if (m_stepPossible[i])
                    {
                        Gizmos.color = Color.blue;
                    }
                    else Gizmos.color = Color.red;

                    Gizmos.DrawCube(m_steps[i], new Vector3(0.4f, 0.4f, 0.4f));
                }
            }
        }

        public void Update()
        {
            if (m_currentAction != null)
            {
                m_currentAction.timeRemaining -= Time.deltaTime;
                if (m_currentAction.timeRemaining < 0)
                {
                    m_currentAction.timeRemaining = 0;
                }

                if (m_playerIsMoving)
                {
                    float t = 1 - (m_currentAction.timeLength - m_currentAction.timeRemaining)/m_currentAction.timeLength;
                    m_transform.position = Vector3.Lerp(m_steps[m_currentStep], m_steps[m_currentStep - 1], t);
                }

                if (m_currentAction.timeRemaining == 0)
                {
                    m_previousAction = m_currentAction;
                    m_currentAction = null;
                    OnMoveEnd();
                }
            }
        }

        public RobotMovement(Transform transform)
        {
            m_transform = transform;
        }
    }

    public delegate void RobotActionDelegate(RobotAction action);
}

