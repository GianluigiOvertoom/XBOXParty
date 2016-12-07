using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }
}
