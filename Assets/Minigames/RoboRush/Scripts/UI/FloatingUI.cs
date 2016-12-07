using UnityEngine;
using System;

namespace RoboRush
{
    public class FloatingUI : MonoBehaviour
    {
        /*==============================
            Because floating is cool
        ==============================*/

        Vector3 beginPosition;
        Vector3 beginAngle;
        Vector3 BeginScale;
        [SerializeField] private float m_revolutionsPerSecond;
        [SerializeField] private float m_amplitude;
        [Space(5f)] // space!
        [SerializeField] private Axis m_axis;
        [SerializeField] private Method m_method;

        public FloatingUI()
        {

        }


        public float revolutionsPerSecond
        {
            get { return m_revolutionsPerSecond; }
            set { m_revolutionsPerSecond = value; }
        }

        public float amplitude
        {
            get { return m_amplitude; }
            set { m_amplitude = value; }
        }

        public Axis axis
        {
            get { return m_axis; }
            set { m_axis = value; }
        }

        public Method method
        {
            get { return m_method; }
            set { m_method = value; }
        }

        // Use this for initialization
        void Start()
        {
            beginPosition = transform.localPosition;
            beginAngle = transform.localRotation.eulerAngles;
            BeginScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            float x = getPosition(-m_amplitude, m_amplitude, Time.time, m_revolutionsPerSecond, m_method);
            setValue(x, m_axis);
        }

        private void setValue(float x, Axis axis)
        {
            switch (axis)
            {
                case Axis.posY:
                    transform.localPosition = new Vector3(0, x, 0) + beginPosition;
                    break;
                case Axis.posX:
                    transform.localPosition = new Vector3(x, 0, 0) + beginPosition;
                    break;
                case Axis.scale:
                    transform.localScale = new Vector3(x, x, (x + m_amplitude) / 2) + BeginScale;
                    break;
                case Axis.angle:
                    transform.localEulerAngles = new Vector3(0, 0, x) + beginAngle;
                    break;
            }
        }

        // Getting the position based on the method we selected.
        private float getPosition(float min, float max, float t, float revolutions, Method method)
        {
            float x;
            switch (m_method)
            {
                case Method.sin: // using a sinwave
                    x = Mathf.Sin((Mathf.Deg2Rad * (t * 360)) * revolutions) * max;
                    break;
                case Method.cos: // using a coswave?
                    x = Mathf.Cos((Mathf.Deg2Rad * (t * 360)) * revolutions) * max;
                    break;
                case Method.saw: // using a sawwave
                    x = Mathf.Lerp(min, max, (t * revolutions) % 1);
                    break;
                case Method.Square: // using a squarewave
                    if (Time.time * m_revolutionsPerSecond % 1 > 0.5f)
                    { x = min; }
                    else { x = max; }
                    break;
                default: // Lets not move when something fucks up.
                    x = 0;
                    break;
            }
            return x;
        }

        [Serializable]
        public enum Axis
        {
            posX,
            posY,
            angle,
            scale
        }

        [Serializable]
        public enum Method
        {
            sin,
            cos,
            saw,
            Square
        }
    }
}

