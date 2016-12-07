using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using XBOXParty;

namespace RoboRush
{
    public class GenericOverlayUI : MonoBehaviour
    {
        public UnityEvent onClose;
        public UnityEvent onOpen;

        [SerializeField]
        private bool m_isOpen = false;
        [SerializeField]
        private bool m_InputControlled = true;

        private Animator m_ani;

        private void Start()
        {
            m_ani = GetComponent<Animator>();
            if (m_isOpen) Open();
        }

        private void Update()
        {
            if (m_InputControlled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Close();
                }

                if (InputManager.Instance)
                {
                    if (InputManager.Instance.GetButton("r_p0A") ||
                        InputManager.Instance.GetButton("r_p1A") ||
                        InputManager.Instance.GetButton("r_p2A") ||
                        InputManager.Instance.GetButton("r_p3A"))
                    {
                        Close();
                    }
                }
            }
        }

        public void Open()
        {
            if (!m_isOpen)
            {
                Debug.Log(name + " has Opened");
                m_isOpen = true;
                if (m_ani)
                    m_ani.SetTrigger("Open");
                onOpen.Invoke();
            }
        }

        public void Close()
        {
            if (m_isOpen)
            {
                Debug.Log(name + " has Closed");
                m_isOpen = false;
                if (m_ani)
                    m_ani.SetTrigger("Close");
                onClose.Invoke();
            }
        }
    }
}

