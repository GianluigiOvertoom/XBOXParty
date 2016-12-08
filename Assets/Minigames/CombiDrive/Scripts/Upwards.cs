using UnityEngine;
using System.Collections;
using XBOXParty;

namespace CombiDrive
{
    public class Upwards : MonoBehaviour
    {
        [SerializeField]
        private float m_HoverHeight = 4.0F;
        [SerializeField]
        private float m_HoverForce = 5.0F;
        [SerializeField]
        private float m_HoverDamp = 0.5F;

        [SerializeField]
        private float m_BouncyHeight = 6;

        [SerializeField]
        private Rigidbody m_Rigidbody;

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            Physics.gravity = new Vector3(0, -50.0f, 0);
        }
        void FixedUpdate()
        {

            m_HoverHeight = Mathf.PingPong(Time.time * 0.8f, 1) + m_BouncyHeight;

            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);

            if (Physics.Raycast(downRay, out hit))
            {
                float hoverError = m_HoverHeight - hit.distance;
                if (hoverError > 0)
                {
                    float upwardSpeed = m_Rigidbody.velocity.y;
                    float lift = hoverError * m_HoverForce - upwardSpeed * m_HoverDamp;
                    m_Rigidbody.AddForce(lift * new Vector3(0, 90, 0));
                }
            }
        }
    }
}
