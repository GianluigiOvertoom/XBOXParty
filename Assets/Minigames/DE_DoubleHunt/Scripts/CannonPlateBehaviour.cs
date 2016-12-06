using UnityEngine;
using System.Collections;
using XBOXParty;

namespace DoubleHunt
{
    public class CannonPlateBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _Target, _Horse;

        private Vector3 _TargetPos;
        private Quaternion _TargetRot;

        void Update()
        {
            transform.position = _Horse.transform.position;
            _TargetPos = new Vector3(_Target.transform.position.x, transform.position.y, _Target.transform.position.z) - transform.position;
            _TargetRot = Quaternion.LookRotation(_TargetPos, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _TargetRot, Time.deltaTime * 3.0f);
        }
    }
}