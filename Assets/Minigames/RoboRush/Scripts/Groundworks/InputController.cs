using UnityEngine;
using System.Collections;
using XBOXParty;

namespace RoboRush
{
    public class InputController : MonoBehaviour
    {
        InputManager m_InputManager;

        int m_playerCount = 4;

        private void Start()
        {
            m_InputManager = InputManager.Instance;
            if (m_InputManager)
                InitInput();
            else
            {
                Debug.LogError(name + ": XboxParty Inputmanager not found. Try starting the minigame through the game board.");
            }
        }

        private void Destroy()
        {
            DisposeInput();
        }

        private void InitInput()
        {
            print("Starting Input");
            m_InputManager = InputManager.Instance;

            // initialize button bindings for each button
            for (int i = 0; i < m_playerCount; i++)
            {
                m_InputManager.BindButton("r_p" + i + "A", i, ControllerButtonCode.A, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "B", i, ControllerButtonCode.B, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "X", i, ControllerButtonCode.X, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "Y", i, ControllerButtonCode.Y, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "Left", i, ControllerButtonCode.Left, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "Right", i, ControllerButtonCode.Right, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "Up", i, ControllerButtonCode.Up, ButtonState.OnPress);
                m_InputManager.BindButton("r_p" + i + "Down", i, ControllerButtonCode.Down, ButtonState.OnPress);
            }
        }

        private void DisposeInput()
        {
            // initialize button bindings for each button
            for (int i = 0; i < m_playerCount; i++)
            {
                m_InputManager.UnbindButton("r_p" + i + "A");
                m_InputManager.UnbindButton("r_p" + i + "B");
                m_InputManager.UnbindButton("r_p" + i + "X");
                m_InputManager.UnbindButton("r_p" + i + "Y");
                m_InputManager.UnbindButton("r_p" + i + "Left");
                m_InputManager.UnbindButton("r_p" + i + "Right");
                m_InputManager.UnbindButton("r_p" + i + "Up");
                m_InputManager.UnbindButton("r_p" + i + "Down");
            }
        }

        public bool getButton(string name)
        {
            return m_InputManager.GetButton(name);
        }

        public float getTrigger(string name)
        {
            return m_InputManager.GetAxis(name);
        }
    }
}