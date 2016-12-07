using UnityEngine;
using System.Collections;

namespace RoboRush
{
    public class Camera : MonoBehaviour
    {
        [SerializeField]
        GameObject m_objectToFollow;

        private float m_distancsX;
        private float m_distanceY;

        private float m_offsetX;
        private float m_offsetY;

        void Start()
        {
            if (m_objectToFollow == null)
            {
                Debug.LogError(name + " does not have an object to follow!");
            }
            else
            {

            }
        }

        void Update()
        {

        }
    }
}
