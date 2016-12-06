using UnityEngine;
using System.Collections;
using XBOXParty;

namespace DoubleHunt
{
    public class Horsemovement : MonoBehaviour
    {
        [SerializeField]
        private float _MForce, _RTorque, _HHeight, _HForce, _HDamp, _ADrag, _Drag;

        [SerializeField]
        private int _PlayerID;

        [SerializeField]
        private Rigidbody _RBody;

        GlobalGameManager _Instance;

        void Start()
        {
            _Instance = GlobalGameManager.Instance;
            InputManager.Instance.BindAxis("DEDH_FMotion" + _PlayerID, _PlayerID, ControllerAxisCode.LeftStickY);
            InputManager.Instance.BindAxis("DEDH_RMotion" + _PlayerID, _PlayerID, ControllerAxisCode.LeftStickX);
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", (Color)_Instance.GetPlayerColor(_PlayerID));
        }

        void FixedUpdate()
        {
            float fmotion = -InputManager.Instance.GetAxis("DEDH_FMotion" + _PlayerID);
            float smotion = InputManager.Instance.GetAxis("DEDH_RMotion" + _PlayerID);

            _RBody.drag = _Drag;
            _RBody.angularDrag = _ADrag;

            _RTorque = _RTorque - (_RBody.velocity.magnitude / 300);
            _RTorque += 1;
            _RTorque = Mathf.Clamp(_RTorque, 1, 5);

            _RBody.AddForce(fmotion * (_MForce - 40 - _RBody.velocity.magnitude) * transform.forward);
            _RBody.AddTorque(smotion * _RTorque * Vector3.up);

            RaycastHit hit;
            Ray dray = new Ray(transform.position, -Vector3.up);

            if (Physics.Raycast(dray, out hit))
            {
                if (hit.transform.tag == "Tag 4")
                {
                    float hoverError = _HHeight - hit.distance;
                    if (hoverError > 0)
                    {
                        float upwardSpeed = _RBody.velocity.y;
                        float lift = hoverError * _HForce - upwardSpeed * _HDamp;
                        _RBody.AddForce(lift * new Vector3(0, 90, 0));
                    }
                }
            }
        }
    }
}