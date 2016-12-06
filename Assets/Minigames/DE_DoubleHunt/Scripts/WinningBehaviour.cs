using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XBOXParty;

namespace DoubleHunt
{
    public class WinningBehaviour : MonoBehaviour
    {

        [SerializeField]
        private string _Team1String, _Team2String;

        [SerializeField]
        private GameObject _Team1Horse, _Team1Marker, _Team1Cannon, _Team2Horse, _Team2Marker, _Team2Cannon;

        [SerializeField]
        private Image _Team1Wins, _Team2Wins;

        private bool _IsGameOver = false;

        private GlobalGameManager _GameManager;

        public List<int> _GameResults;

        void Start()
        {
            InputManager.Instance.BindButton("DEDH_SubmitScore", 0, ControllerButtonCode.A, ButtonState.OnPress);
            _GameManager = GlobalGameManager.Instance;
            _GameResults = new List<int>();
        }

        void Update()
        {
            if (_IsGameOver && InputManager.Instance.GetButton("DEDH_SubmitScore"))
            {
                _GameManager.SubmitGameResults(_GameResults);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == _Team1String)
            {
                if (!_IsGameOver)
                {
                    Destroy(_Team1Horse);
                    Destroy(_Team1Marker);
                    Destroy(_Team1Cannon);
                    _Team2Wins.gameObject.SetActive(true);
                    _GameResults.Add(1);
                    _GameResults.Add(0);
                    _GameResults.Add(0);
                    _GameResults.Add(1);
                    _IsGameOver = true;
                }
            }
            
            if (other.transform.tag == _Team2String)
            {
                if (!_IsGameOver)
                {
                    Destroy(_Team2Horse);
                    Destroy(_Team2Marker);
                    Destroy(_Team2Cannon);
                    _Team1Wins.gameObject.SetActive(true);
                    _GameResults.Add(0);
                    _GameResults.Add(1);
                    _GameResults.Add(1);
                    _GameResults.Add(0);
                    _IsGameOver = true;
                }
            }
        }
    }
}
