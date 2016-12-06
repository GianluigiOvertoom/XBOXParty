using UnityEngine;
using System.Collections;

namespace KeenKeld
{
    public class Coin : PoolableObject
    {
        [SerializeField]
        private bool m_destroyOnGround = true;
        [SerializeField]
        private float m_destroyAfter = 3f, m_canGrabAfter = 3f;
        [SerializeField]
        private Renderer m_renderer;
        [Tooltip("<0.5 = deactivate | >=0.5 = active")]
        [SerializeField]
        private AnimationCurve m_flickerCurve;
        [SerializeField]
        private Vector3 m_spinSpeed = new Vector3();

        private bool m_grounded = false, m_canGrab = false;

        private float m_timer;
        

        void Start()
        {
            if (m_renderer == null)
                m_renderer = gameObject.GetComponent<Renderer>();

            m_renderer.enabled = true;
        }

        void OnEnable ()
        {
            m_timer = 0f;
            m_grounded = false;
            m_canGrab = false;
            m_renderer.enabled = true;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        void Update()
        {
            transform.Rotate(m_spinSpeed * Time.deltaTime);

            if(!m_canGrab)
            {
                m_timer -= Time.deltaTime;

                AnimateCoin(1 - (m_timer / m_canGrabAfter));

                if (m_timer <= m_canGrabAfter)
                {
                    m_canGrab = true;
                    m_renderer.enabled = true;
                    m_timer = 0f;
                }
            }



            if(m_canGrab && m_grounded)
            {
                m_timer += Time.deltaTime;

                AnimateCoin(m_timer / m_destroyAfter);

                if (m_timer >= m_destroyAfter)
                    gameObject.SetActive(false);
            }
        }

        void AnimateCoin (float time)
        {
            if (m_flickerCurve.Evaluate(time) < .5f)
                m_renderer.enabled = false;
            else
                m_renderer.enabled = true;
        }

        void OnCollisionEnter (Collision collision)
        {
                if (collision.transform.tag != "Player")
                {
                    m_grounded = true;
                }
                else if (m_canGrab)
                {
                    collision.gameObject.GetComponent<Player>().GrabbedCoin();
                    gameObject.SetActive(false);
                }
        }

        public override void Activate ()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate ()
        {
            gameObject.SetActive(false);
        }

        public override bool IsActive ()
        {
            return gameObject.activeSelf;
        }
    }
}
