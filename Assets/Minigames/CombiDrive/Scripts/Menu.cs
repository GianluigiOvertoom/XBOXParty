using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XBOXParty;

namespace CombiDrive
{
    public class Menu : MonoBehaviour
    {
        private void Awake()
        {
            InputManager.Instance.BindButton("CombiDrive_Continue_0", 0 , ControllerButtonCode.A, ButtonState.Pressed);
        }

        private void Update()
        {
            if (InputManager.Instance.GetButton("CombiDrive_Continue_0"))
            {
                SceneManager.LoadScene("RaceTrackScene");
            }
        }
    }
}
