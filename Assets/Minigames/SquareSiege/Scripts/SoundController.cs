using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private int m_AudioSourceAmount = 8;
    public int AudioSourceAmount { get { return m_AudioSourceAmount; } set { m_AudioSourceAmount = value; } }

    private AudioSource[] m_AudioSources;

    void Awake()
    {
        m_AudioSources = new AudioSource[m_AudioSourceAmount];

        for (int i = 0; i < m_AudioSources.Length; i++)
        {
            m_AudioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound( AudioClip clip, bool loop = false )
    {
        AudioSource source = GetInactiveAudioSource();
        source.clip = clip;
        source.volume = .5f;
        source.loop = loop;
        source.Play();
    }

    public void PlaySound( AudioClip clip, int priority )
    {
        AudioSource source = m_AudioSources[Mathf.Clamp(priority, 0, m_AudioSources.Length)];
        source.clip = clip;
        source.volume = .5f;
        source.Play();
    }

    private AudioSource GetInactiveAudioSource()
    {
        foreach (AudioSource source in m_AudioSources)
        {
            if (!source.isPlaying) return source;
        }

        Debug.Log("All audio sources are active!");
        return m_AudioSources[m_AudioSources.Length - 1];
    }
}
