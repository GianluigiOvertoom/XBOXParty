using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using XBOXParty;

namespace RoboRush
{
    public class TimedOverlay : MonoBehaviour
    {
        public float m_delayOnOpen = 4;
        public float m_delayOnClose = 4;

        public UnityEvent onClose;
        public UnityEvent onOpen;

        private bool m_isOpen = false;
        private bool m_isRun = false;

        private Animator m_ani;

        private void Start()
        {
            m_ani = GetComponent<Animator>();
        }

        private void Update()
        {
            if (m_isRun)
            {
                if (m_isOpen == false)
                {
                    if (m_delayOnOpen > 0)
                    {
                        m_delayOnOpen -= Time.deltaTime;
                    }
                    else
                    {
                        Show();
                    }
                }
                else
                {
                    if (m_delayOnClose > 0)
                    {
                        m_delayOnClose -= Time.deltaTime;
                    }
                    else
                    {
                        Hide();
                    }
                }
            }
        }

        public void Show()
        {
            if (!m_isOpen)
            {
                m_isOpen = true;
                if (m_ani)
                    m_ani.SetTrigger("Open");
                onOpen.Invoke();
            }
        }

        public void Hide()
        {
            if (m_isOpen)
            {
                m_isOpen = false;
                if (m_ani)
                    m_ani.SetTrigger("Close");
                onClose.Invoke();
                enabled = false;
            }
        }

        public void Run()
        {
            m_isRun = true;
        }
    }
}

