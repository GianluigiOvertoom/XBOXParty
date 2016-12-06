using UnityEngine;
using System.Collections;

namespace DoubleHunt
{
    public class GroundBehaviour : MonoBehaviour
    {

        public float _Health = 4f;
        public Material _MT;
        private Renderer _Rend;

        void Start()
        {
            _Rend = GetComponent<Renderer>();
        }

        void Update()
        {
            if (_Health <= 1)
            {
                _Rend.material = _MT;
            }

            if (_Health <= 0)
            {
                destroyme();
            }
        }

        void destroyme()
        {
            Destroy(gameObject);
        }
    }
}