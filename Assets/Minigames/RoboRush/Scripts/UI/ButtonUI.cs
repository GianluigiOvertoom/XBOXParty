using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RoboRush
{
    public class ButtonUI : MonoBehaviour
    {
        private Animator m_animator;
        private bool isFirst = true;

        [SerializeField]
        private Image m_Image;

        [SerializeField]
        private Sprite[] m_PlayerSprites;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public void SetPlayer(int player,Color color)
        {
            Debug.Log(name +": "+ player.ToString() + " " + color.ToString());
            m_Image.sprite = m_PlayerSprites[player];
            m_Image.color = color;
        }

        public void ChangeText(ControllerButtons button)
        {
            if (!isFirst)
            {
                m_animator.SetTrigger("Close");
            }
            else isFirst = false;

            m_animator.SetTrigger(button.ToString().ToUpper());
        }

        public void Close()
        {
            m_animator.SetTrigger("Close");
        }
    }
}

