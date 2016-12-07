using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 m_OriginalPos;
    private float m_TimeAtCurrentFrame;
    private float m_TimeAtLastFrame;
    private float m_FakeDelta;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        m_TimeAtCurrentFrame = Time.realtimeSinceStartup;
        m_FakeDelta = m_TimeAtCurrentFrame - m_TimeAtLastFrame;
        m_TimeAtLastFrame = m_TimeAtCurrentFrame;
    }

    public void Shake( float duration, float amount )
    {
        instance.m_OriginalPos = instance.gameObject.transform.localPosition;
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    private IEnumerator cShake( float duration, float amount )
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            transform.localPosition = m_OriginalPos + Random.insideUnitSphere * amount * duration;

            duration -= m_FakeDelta;

            yield return null;
        }

        transform.localPosition = m_OriginalPos;
    }
}
