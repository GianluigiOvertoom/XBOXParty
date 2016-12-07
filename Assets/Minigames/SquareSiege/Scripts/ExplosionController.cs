using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
    private CameraShake m_CameraShaker;

    void Awake()
    {
        m_CameraShaker = FindObjectOfType<CameraShake>();
    }

    void Start()
    {
        m_CameraShaker.Shake(.3f, .05f);
    }
}
