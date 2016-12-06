using UnityEngine;
using System.Collections;

namespace KeenKeld
{
    public class BarrierFadeMaterial : MonoBehaviour
    {
        [SerializeField]
        private GameManager m_gameManager;
        public Transform[] m_players;
        private Renderer m_renderer;
        [SerializeField]
        private bool m_closestX, m_closestY, m_closestZ;
        [SerializeField]
        private float m_minDistance = 5f;
        [SerializeField]
        private Gradient m_gradient;

        private float m_closestDistance = 0f;

        void Start ()
        {
            m_renderer = gameObject.GetComponent<Renderer>();
            m_players = m_gameManager.players.ToArray();
        }

        void Update()
        {
            Transform closestPlayer;

            if (m_players != null)
            {
                closestPlayer = GetClosestPlayer();
                m_renderer.material.SetColor("_TintColor", m_gradient.Evaluate(Mathf.InverseLerp(0, m_minDistance, m_closestDistance)));
            }
            else
            {
                m_renderer.material.SetColor("_TintColor", m_gradient.Evaluate(1));
            }
        }

        private Transform GetClosestPlayer()
        {
            Transform closest = null;
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < m_players.Length; i++)
            {
                Vector3 pos = m_players[i].position;

                if (!m_closestX)
                    pos.x = transform.position.x;
                if (!m_closestY)
                    pos.y = transform.position.y;
                if (!m_closestZ)
                    pos.z = transform.position.z;

                Vector3 direction = pos - transform.position;
                float distance = direction.magnitude;

                if(distance < closestDist)
                {
                    closestDist = distance;
                    closest = m_players[i];
                    m_closestDistance = distance;
                }
            }

            return closest;
        }
    }
}