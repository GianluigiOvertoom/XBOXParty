using UnityEngine;
using System.Collections;
using XBOXParty;

namespace DoubleHunt
{
    public class MarkerBehaviour : MonoBehaviour
    {

        [SerializeField]
        private float _Length;

        [SerializeField]
        private GameObject _Horse, _BombPrefab;

        [SerializeField]
        private int _PlayerID;

        private Vector3 _Move = Vector3.zero;
        GlobalGameManager _Instance;
        public bool _StillInAir = false;

        void Start()
        {
            InputManager.Instance.BindAxis("DEDH_MarkerUpDown" + _PlayerID, _PlayerID, ControllerAxisCode.LeftStickY);
            InputManager.Instance.BindAxis("DEDH_MarkerLeftRight" + _PlayerID, _PlayerID, ControllerAxisCode.LeftStickX);
            InputManager.Instance.BindButton("DEDH_Shoot" + _PlayerID, _PlayerID, ControllerButtonCode.A, ButtonState.OnPress);
            _Instance = GlobalGameManager.Instance;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", (Color)_Instance.GetPlayerColor(_PlayerID));
        }

        void Update()
        {
            _Move = new Vector3(InputManager.Instance.GetAxis("DEDH_MarkerLeftRight" + _PlayerID), 0, InputManager.Instance.GetAxis("DEDH_MarkerUpDown" + _PlayerID));
            _Move *= _Length;

            transform.position = _Move + new Vector3(_Horse.transform.position.x, 1, _Horse.transform.position.z);

            if (InputManager.Instance.GetButton("DEDH_Shoot" + _PlayerID) && _StillInAir == false)
            {
                _StillInAir = true;
                Instantiate(_BombPrefab, new Vector3(transform.position.x, 20, transform.position.z), transform.rotation);
            }
        }
    }
}
