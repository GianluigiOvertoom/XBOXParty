using UnityEngine;
using System.Collections;
using XBOXParty;

namespace RoboRush
{
    public class RobotAnimation
    {

        private Animator m_animator;
        private bool m_IsPlaying = false;
        private RobotAction m_CurrentAnimation;

        public RobotAnimation(Animator animator)
        {
            m_animator = animator;
        }

        public void Update()
        {
            if (m_CurrentAnimation != null)
            {
                if (m_CurrentAnimation.timeRemaining > 0)
                {
                    m_CurrentAnimation.timeRemaining -= Time.deltaTime;
                }
            }
        }

        public void PlayAnimation(RobotAction animation)
        {
                m_CurrentAnimation = animation;
                TriggerAnimation(m_CurrentAnimation.actionType);
        }

        private void TriggerAnimation(RobotActionType type)
        {
            switch (type)
            {
                case RobotActionType.Jump:
                {
                    m_animator.SetTrigger("Jump");
                }
                break;

                case RobotActionType.StaggerLeft:
                case RobotActionType.StepLeft:
                {
                    m_animator.SetTrigger("StepLeft");
                }
                break;

                case RobotActionType.StaggerRight:
                case RobotActionType.StepRight:
                {
                    m_animator.SetTrigger("StepRight");
                }
                break;

                case RobotActionType.Win:
                    m_animator.SetTrigger("Win");
                break;
                case RobotActionType.Lose:
                    m_animator.SetTrigger("Lose");
                break;

                default:break; // If its another animation, don't do anything.
            }
        }
    }
}