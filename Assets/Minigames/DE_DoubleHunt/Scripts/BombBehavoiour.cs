using UnityEngine;
using System.Collections;

namespace DoubleHunt
{
    public class BombBehavoiour : MonoBehaviour
    {
        public GameObject _Prefab, _SoundPrefab, _Marker;
        public string _MarkerTag;

        void Awake()
        {
            _Marker = GameObject.FindGameObjectWithTag(_MarkerTag);
        }

        void OnCollisionEnter(Collision Collideddwith)
        {
            GroundBehaviour collidewith = Collideddwith.gameObject.GetComponent<GroundBehaviour>();
            if (collidewith == null)
                return;

            _Marker.GetComponent<MarkerBehaviour>()._StillInAir = false;
            if (collidewith.GetComponent<GroundBehaviour>() != null)
            {
                collidewith._Health--;
                Instantiate(_Prefab, gameObject.transform.position, Quaternion.Euler(transform.rotation.x + 90, 0, 0));
                Instantiate(_SoundPrefab, gameObject.transform.position, Quaternion.Euler(transform.rotation.x + 90, 0, 0));
                Destroy(gameObject);
            }
            if(Collideddwith.gameObject.GetComponent<GroundBehaviour>() == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
