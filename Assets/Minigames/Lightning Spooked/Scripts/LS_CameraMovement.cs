using UnityEngine;
using System.Collections;

namespace LightningSpooked
{
    public class LS_CameraMovement : MonoBehaviour
    {
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Tag3"))
            {   
                other.gameObject.GetComponent<MeshRenderer>().material.color = new Color(156 / 255, 73 / 255, 73 / 255, 30 / 255);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Tag3"))
            {
                other.gameObject.GetComponent<MeshRenderer>().material.color = new Color(156 / 255, 73 / 255, 73 / 255, 255 / 255);
            }
        }
    }
}