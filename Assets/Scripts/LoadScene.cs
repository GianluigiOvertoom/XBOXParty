using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

	void Start ()
    {
        Application.LoadLevel(_sceneName);
	}
	
}
