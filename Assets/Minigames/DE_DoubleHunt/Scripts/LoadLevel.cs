using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XBOXParty;

namespace DoubleHunt
{
    public class LoadLevel : MonoBehaviour {

        [SerializeField]
        private string _LevelName;

        void Start()
        {
            InputManager.Instance.BindButton("DEDH_StartLevel", 0, ControllerButtonCode.A, ButtonState.OnPress);
        }

        void Update()
        {
            if (InputManager.Instance.GetButton("DEDH_StartLevel"))
            {
                SceneManager.LoadScene(_LevelName);
            }
        }
    }
}