using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XBOXParty;

namespace Board
{
    [RequireComponent (typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class MinigameSelectionIcon : MonoBehaviour
    {
        [SerializeField]
        private MinigameSelectionMenu m_MinigameSelectionMenu;

        [SerializeField]
        private float m_MaxSpeed = 1.0f;
        private float m_CurrentSpeed = 1.0f;

        [SerializeField]
        private float m_Acceleration = 1.0f;

        [SerializeField]
        private float m_StopSpeed = 1.0f;

        [SerializeField]
        private RectTransform m_RespawnLocation;

        [SerializeField]
        private RectTransform m_SpawnLocation;
        private RectTransform m_MyTransform;
        private Image m_Image;

        private bool m_IsRolling = false;
        public bool IsRolling
        {
            get { return m_IsRolling; }
        }

        private bool m_IsStopping = false;
        public bool IsStopping
        {
            get { return m_IsStopping; }
        }

        private bool m_IsChosen = false;
        public bool IsChosen
        {
            get { return m_IsChosen; }
        }

        private MinigameData m_MinigameData;

        private void Awake()
        {
            m_MyTransform = GetComponent<RectTransform>();
            m_Image = GetComponent<Image>();
        }

        private void Update()
        {
            HandleStopping();
            HandleRolling();
            HandleScale();
            HandleSelection();
        }

        private void HandleStopping()
        {
            if (!m_IsStopping || !m_IsRolling)
                return;

            //Lower the speed
            m_CurrentSpeed -= m_StopSpeed * Time.deltaTime;

            if (m_CurrentSpeed < 0.0f)
            {
                m_CurrentSpeed = 0.0f;
                m_IsRolling = false;

                //Set ourselves to the closest multiple of halfwidth
                int multiple = Mathf.RoundToInt(m_MyTransform.anchoredPosition.x / m_MyTransform.sizeDelta.x);

                float newX = multiple * m_MyTransform.sizeDelta.x;
                float newY = m_MyTransform.anchoredPosition.y;

                m_MyTransform.anchoredPosition = new Vector2(newX, newY);

                //If we were chosen:
                if (m_IsChosen)
                {
                    SetRandomAngle(2, 5);
                    m_MyTransform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
                }
            }
        }

        private void HandleRolling()
        {
            if (!m_IsRolling)
                return;

            //Accelerate
            if (m_CurrentSpeed < m_MaxSpeed && m_IsStopping == false)
            {
                m_CurrentSpeed += m_Acceleration * Time.deltaTime;
                if (m_CurrentSpeed > m_MaxSpeed) { m_CurrentSpeed = m_MaxSpeed; }
            }

            //Calculate movement
            float newX = m_MyTransform.anchoredPosition.x - m_CurrentSpeed * Time.deltaTime;
            float newY = m_MyTransform.anchoredPosition.y;

            //Clamp movement
            if (newX < m_RespawnLocation.anchoredPosition.x)
            {
                //Reset position
                newX = m_SpawnLocation.anchoredPosition.x - (Mathf.Abs(newX) - Mathf.Abs(m_RespawnLocation.anchoredPosition.x));

                //Set to the front of the row
                m_MyTransform.SetAsFirstSibling();

                //Hold new minigame data
                SetMinigameData(m_MinigameSelectionMenu.GetNextMinigame());
            }

            //Actually move
            m_MyTransform.anchoredPosition = new Vector2(newX, newY);
        }

        private void HandleScale()
        {
            if (!m_IsRolling)
                return;

            //Set their scale according to their current position
            float scale = Mathf.Lerp(1.5f, 0.8f, Mathf.Abs(m_MyTransform.anchoredPosition.x) / m_SpawnLocation.anchoredPosition.x);
            m_MyTransform.localScale = new Vector3(scale, scale, 1.0f);
        }

        private void HandleSelection()
        {
            m_IsChosen = false;

            //If we are less than half our width away from 0 we were the chosen minigame!
            float halfWidth = m_MyTransform.sizeDelta.x * 0.5f;

            if (Mathf.Abs(m_MyTransform.anchoredPosition.x) < halfWidth ||
                          m_MyTransform.anchoredPosition.x == halfWidth)
            {
                //Place ourselves visually on top
                if (!m_IsChosen)
                {
                    m_MyTransform.SetAsLastSibling();
                }

                Debug.Log(gameObject.name + " was the chosen minigame!");
                m_IsChosen = true;
            }
        }

        private void SetRandomAngle(int min, int max)
        {
            //Random angle
            float randomAngle = Random.Range(min, max);
            float randomSign = Random.Range(0, 100);

            if (randomSign >= 50) { randomAngle *= -1; }

            m_MyTransform.Rotate(0.0f, 0.0f, randomAngle);
        }

        public void StartRolling()
        {
            m_IsRolling = true;
            m_IsStopping = false;

            m_MyTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        public void StopRolling()
        {
            m_IsStopping = true;
        }

        public MinigameData GetMinigameData()
        {
            return m_MinigameData;
        }

        public void SetMinigameData(MinigameData minigameData)
        {
            m_MinigameData = minigameData;
            m_Image.sprite = m_MinigameData.Thumbnail;
        }
    }
}
