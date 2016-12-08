using UnityEngine;
using System.Collections;

namespace CombiDrive
{
    public class PowerUps : MonoBehaviour
    {
        [SerializeField]
        private Material m_Material;

        private float m_Red;
        private float m_Green;
        private float m_Blue;

        private void Awake()
        {
            m_Material = GetComponent<Renderer>().material;

        }

        private void Update()
        {
            m_Red = Mathf.PingPong(Time.time, 1f);
            m_Blue = Mathf.PingPong(Time.time * 2, 1f);
            m_Green = Mathf.PingPong(Time.time * 3.6f, 1f);

            m_Material.SetColor(Shader.PropertyToID("_EmissionColor"), new Color(m_Red, m_Green, m_Blue));
        }
    }
}
