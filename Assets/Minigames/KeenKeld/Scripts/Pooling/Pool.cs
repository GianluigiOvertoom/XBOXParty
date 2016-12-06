using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KeenKeld
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private PoolableObject m_poolableObject;
        [SerializeField]
        private int m_spawnAmount = 10;
        private List<PoolableObject> m_pool = new List<PoolableObject>();


        void Awake()
        {
            for (int i = 0; i < m_spawnAmount; i++)
            {
                PoolableObject b = Instantiate(m_poolableObject, Vector3.zero, Quaternion.identity) as PoolableObject;
                b.transform.SetParent(transform);
                m_pool.Add(b);
                b.Deactivate();
            }
        }


        public void Instantiate(Vector3 pos, Quaternion rot)
        {
            for (int i = 0; i < m_pool.Count; i++)
            {
                if (!m_pool[i].IsActive())
                {
                    m_pool[i].transform.position = pos;
                    m_pool[i].transform.rotation = rot;
                    m_pool[i].Activate();
                    break;
                }
            }
        }
    }
}