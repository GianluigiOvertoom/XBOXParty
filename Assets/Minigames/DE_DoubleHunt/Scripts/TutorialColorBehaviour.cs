using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XBOXParty;

namespace DoubleHunt
{
    public class TutorialColorBehaviour : MonoBehaviour
    {
        [SerializeField]
        private int _PlayerID;

        GlobalGameManager _Instance;


        void Start()
        {
            _Instance = GlobalGameManager.Instance;
            gameObject.GetComponent<Image>().color = _Instance.GetPlayerColor(_PlayerID);
        }
    }
}