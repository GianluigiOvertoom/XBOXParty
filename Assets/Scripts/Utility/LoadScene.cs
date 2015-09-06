using UnityEngine;
using System.Collections;

namespace Board
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;

        void Start()
        {
            Application.LoadLevel(_sceneName);
        }

    }
}
