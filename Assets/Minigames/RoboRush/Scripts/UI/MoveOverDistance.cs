using UnityEngine;
using System.Collections;

namespace RoboRush
{
    public class MoveOverDistance : MonoBehaviour
    {
        [SerializeField]
        private Robot m_player;

        private float m_distance;
        private float m_distanceTraveled;

        [SerializeField]
        private float m_startposition;
        [SerializeField]
        private float m_endPosition;

        [Range(0f,1f)]
        public float pos;

        void Start()
        {
            m_distance = m_player.distanceToTravel;
            if (!m_player)
            {
                enabled = false;
                Debug.LogError(name + " doesn't contain a reference to a player!!");
            }
        }

        void Update()
        {
            m_distanceTraveled = m_player.distanceTraveled;
            float x = transform.localPosition.x;
            float y = transform.localPosition.y;
            float z = transform.localPosition.z;
            //transform.localPosition = new Vector3(Mathf.Lerp(m_startposition, m_endPosition,pos),y,z);

            transform.localPosition = new Vector3(Mathf.Lerp(m_startposition, m_endPosition,Mathf.Clamp01(1 - ((m_distanceTraveled / m_distance) - 1))), y, z);
        }
    }

}
