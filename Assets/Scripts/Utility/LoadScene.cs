using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Board
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;

        void Start()
        {
            SceneManager.LoadScene(_sceneName);
            //Application.LoadLevel(_sceneName);
        }

    }
}
