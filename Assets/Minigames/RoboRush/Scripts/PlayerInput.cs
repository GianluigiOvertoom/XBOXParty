using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using XBOXParty;

namespace RoboRush
{
    public class PlayerInput
    {
        public event ButtonUpdateDelegate OnButtonChange;
        public event VoidDelegate OnButtonPressed;

        private ControllerButtons m_PreviousButton;
        public ControllerButtons previousButton
        {
            get
            {
                return m_PreviousButton;
            }
        }

        private int m_PlayerNumber = 0;
        public int playerNumber
        {
            get
            {
                return m_PlayerNumber;
            }
        }

        public PlayerInput(int playerNumber)
        {
            m_PlayerNumber = playerNumber;
        }

        // Get a new button from ControllerButtons Enum
        private ControllerButtons GetNewButton()
        {
            Array values = Enum.GetValues(typeof(ControllerButtons));
            m_PreviousButton = (ControllerButtons)values.GetValue((int)Random.Range(0, values.Length));
            OnButtonChange.Invoke(m_PreviousButton);
            return m_PreviousButton;
        }

        public void ForceButtonChange()
        {
            GetNewButton();
        }

        public void CheckButton()
        {
            if (IsButtonPressed())
            {
                OnButtonPressed.Invoke();
                GetNewButton();
            }
        }

        private bool IsButtonPressed()
        {
            switch (m_PreviousButton)
            {
                case ControllerButtons.A:
                {
                    return InputManager.Instance.GetButton("r_p" + playerNumber + "A");
                }
                case ControllerButtons.B:
                {
                    return InputManager.Instance.GetButton("r_p" + playerNumber + "B");
                }
                case ControllerButtons.X:
                {
                    return InputManager.Instance.GetButton("r_p" + playerNumber + "X");
                }
                case ControllerButtons.Y:
                {
                    return InputManager.Instance.GetButton("r_p" + playerNumber + "Y");
                }
                default: return true;
            }
        }
    }

    public delegate void ButtonUpdateDelegate(ControllerButtons NewButton);

    public enum ControllerButtons
    {
        A,
        B,
        X,
        Y,
    }
}
