using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SquareSiege
{
    public class MarkerController : MonoBehaviour
    {
        private TeamController m_TeamController;

        private float m_YPosition;
        private float m_YFloatAmplitude = .2f;
        private float m_YFloatSpeed = 2.0f;

        private Stack<GameObject> m_HealthBalls;

        void Awake()
        {
            m_TeamController = transform.parent.GetComponent<TeamController>();
        }

        // Use this for initialization
        void Start()
        {
            m_YPosition = transform.position.y;

            m_HealthBalls = new Stack<GameObject>();

            for (int i = 0; i < m_TeamController.MaxLives; i++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.GetComponent<MeshRenderer>().material.color = Color.red;
                go.GetComponent<SphereCollider>().enabled = false;
                go.transform.SetParent(transform);
                go.transform.localScale = new Vector3(.05f, .3f, .3f);
                go.transform.localPosition = new Vector3(i * .5f - (.5f * Mathf.Floor(m_TeamController.MaxLives / 2)), 1f, .0f);

                m_HealthBalls.Push(go);
            }
        }

        // Update is called once per frame
        void Update()
        {
            float y = m_YPosition + m_YFloatAmplitude * Mathf.Sin(m_YFloatSpeed * Time.time);

            transform.position = new Vector3(m_TeamController.Tank.transform.position.x, y, m_TeamController.Tank.transform.position.z);

            transform.Rotate(Vector3.up * Mathf.Sin(3 * Time.time));
        }

        public void RemoveHealth( int amount = 1 )
        {
            for (int i = 0; i < amount; i++)
            {
                Destroy(m_HealthBalls.Pop());
            }
        }
    }
}
