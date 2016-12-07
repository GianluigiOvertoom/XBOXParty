using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using XBOXParty;


/// <summary>
/// All the code in here is disgusting.
/// Stop looking at it..
/// 
/// I am sad.
/// </summary>
namespace RoboRush
{

    public delegate void IntDelegate(int integer);
    public class Robot : MonoBehaviour
    {
        public event IntDelegate UponReachingFinish;
        public event VoidDelegate OnActionStart;
        public event VoidDelegate OnACtionEnd;

        [SerializeField]
        private float m_Timer = 0;
        [SerializeField]
        private bool m_IsPlayingAnimation = false;
        [Space(5f)]
        [SerializeField]
        private float m_AnimationTimeStep = 0;
        [SerializeField]
        private float m_AnimationTimeJump = 0;
        [SerializeField]
        private float m_AnimationTimeStagger = 0;
        [SerializeField]
        private float m_AnimationDelay = 0;
        [Space(5f)]
        [SerializeField]
        private float m_DistanceStep = 0;
        [SerializeField]
        private int m_StepAmount = 100;

        [Range(0,1)]
        [SerializeField]
        private int playernumber = 0;

        private RobotMovement m_robotMove;
        private RobotAnimation m_robotAnimation;

        [SerializeField]
        private RobotAction m_previewAction;
        private RobotAction m_previousAction;
        private RobotAction m_nextAction;

        private bool m_isLeftButtonPressed = false;
        private bool m_isRightButtonPressed = false;

        private float m_timeBeforeNextAction = 0;

        private Vector3 m_endposition;

        public float distanceToTravel
        {
            get; private set;
        }

        public float distanceTraveled
        {
            get; private set;
        }


        private void Awake()
        {
            distanceToTravel = m_DistanceStep * (m_StepAmount-2);
            m_endposition = transform.position + new Vector3(distanceToTravel, 0, 0);
        }

        private void Start()
        {
            m_robotMove = new RobotMovement(this.transform);
            m_robotAnimation = new RobotAnimation(GetComponent<Animator>());

            m_robotMove.OnMoveEnd += MoveComplete;
            m_robotMove.OnMoveEnd += CheckIfWon;

            UponReachingFinish += Win;

            List<Vector3> list = new List<Vector3>();

            for (int i = 0; i < m_StepAmount; i++)
            {
                list.Add(transform.position + new Vector3(i * m_DistanceStep, 0, 0));
            }

            m_robotMove.PrecalculatePath(list.ToArray());

        }

        private void CheckIfWon()
        {
            if (m_robotMove.isOnLastMove)
            {
                UponReachingFinish(playernumber);
            }
        }

        public void Win(int playernumer)
        {
            m_robotAnimation.PlayAnimation(new RobotAction(0, RobotActionType.Win));
        }

        public void Lose()
        {
            m_robotAnimation.PlayAnimation(new RobotAction(0, RobotActionType.Lose));
        }

        public void MoveComplete()
        {
            m_IsPlayingAnimation = false;

            if (OnACtionEnd != null)
            OnACtionEnd.Invoke();
        }

        private void Update()
        {

            distanceTraveled = distanceToTravel + Vector3.Distance(transform.position, m_endposition);

            // [TODO] Remove Action Preview
            if (m_nextAction != null)
            {
                m_previewAction = m_nextAction;
            }

            m_Timer = m_timeBeforeNextAction;
            if (m_nextAction != null)
            {
                m_timeBeforeNextAction -= Time.deltaTime;

                if (m_timeBeforeNextAction <= 0)
                {
                    m_isLeftButtonPressed = false;
                    m_isRightButtonPressed = false;

                    m_IsPlayingAnimation = true;
                    if (m_previousAction != null)
                    {
                        if (m_nextAction.actionType == m_previousAction.actionType)
                        {
                            if (m_previousAction.actionType == RobotActionType.StepLeft)
                            {
                                m_nextAction = new RobotAction(m_AnimationTimeStagger, RobotActionType.StaggerLeft);
                            }
                            else m_nextAction = new RobotAction(m_AnimationTimeStagger, RobotActionType.StaggerRight);
                        }
                    }
                    m_robotAnimation.PlayAnimation(m_nextAction);
                    m_robotMove.performAction(m_nextAction);
                    if (m_nextAction.actionType == RobotActionType.StepRight || m_nextAction.actionType == RobotActionType.StepLeft)
                    {
                        m_previousAction = m_nextAction;
                    }
                    m_previewAction = m_nextAction;
                    m_nextAction = null;
                    if (OnActionStart != null)
                    OnActionStart.Invoke();
                }
            }

            m_robotMove.Update();
            m_robotAnimation.Update();
        }

        public void MoveLeft()
        {
            if (m_IsPlayingAnimation) return;
            m_isLeftButtonPressed = true;
            RobotAction input = new RobotAction(m_AnimationTimeStep, RobotActionType.StepLeft);
            AddAction(input);
        }

        public void MoveRight()
        {
            if (m_IsPlayingAnimation) return;
            m_isRightButtonPressed = true;
            RobotAction input = new RobotAction(m_AnimationTimeStep, RobotActionType.StepRight);
            AddAction(input);
        }


        private void AddAction(RobotAction action)
        {

            if (m_nextAction == null) ResetTimer();

            if (m_robotMove.nextMovePossible)
            {
                if (action.actionType == RobotActionType.StepLeft)
                {
                    if (m_isRightButtonPressed)
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeJump, RobotActionType.Jump);
                    }
                    else
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeStep, RobotActionType.StepLeft);
                    }
                }
                else
                {
                    if (m_isLeftButtonPressed)
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeJump, RobotActionType.Jump);
                    }
                    else
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeStep, RobotActionType.StepRight);
                    }
                }
            }
            else
            {
                if (action.actionType == RobotActionType.StepLeft)
                {
                    if (m_isRightButtonPressed)
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeJump, RobotActionType.Jump);
                    }
                    else
                    {
                        m_nextAction = m_previousAction;
                    }
                }
                else
                {
                    if (m_isLeftButtonPressed)
                    {
                        m_nextAction = new RobotAction(m_AnimationTimeJump, RobotActionType.Jump);
                    }
                    else
                    {
                        m_nextAction = m_previousAction;
                    }
                }
            }
        }

        private void ResetTimer()
        {
            m_timeBeforeNextAction = m_AnimationDelay;
        }

        /// <summary>
        /// Drawing all positions on the field
        /// This is for the artists to see where 
        /// to approximately place down obstacles.
        /// </summary>
        void OnDrawGizmos()
        {
            if (m_robotMove == null)
            {
                for (int i = 0; i < m_StepAmount; i++)
                {
                    Gizmos.color = new Color(1, 1, 1);
                    Gizmos.DrawCube(transform.position + new Vector3(i * m_DistanceStep, 0.5f, 0), new Vector3(0.2f, 0.2f, 0.2f));
                }
            }
            else
            {
                m_robotMove.OnGizmoDraw();
            }
        }
    }

    [Serializable]
    public class RobotAction
    {
        public float timeRemaining;
        public float timeLength;
        public RobotActionType actionType;

        public RobotAction(float time, RobotActionType type)
        {
            timeRemaining = time;
            timeLength = time;
            actionType = type;
        }
    }

    public enum RobotActionType
    {
        StepLeft,
        StepRight,
        StaggerLeft,
        StaggerRight,
        Idle,
        Win,
        Lose,
        Jump
    }
}
