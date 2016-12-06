using UnityEngine;
using System.Collections;
using XBOXParty;
using UnityEngine.UI;

namespace KeenKeld
{
    public class Player : MonoBehaviour
    {
        private GameManager m_gameManager;
        [SerializeField]
        private int m_playerID = 0;
        public int playerID { get { return m_playerID; } set { m_playerID = value; } }
        [SerializeField]
        private Renderer m_wallet;

        private Rigidbody m_rigidbody;
        private AudioSource m_audioSource;

        [SerializeField]
        private float m_moveSpeed = 8f, m_maxVelocityChange = 10f, m_turnSpeed = 9f, m_gravity = 10f, m_jumpHeight = 5f, m_checkGroundedLength = 2.2f, m_stompSpeed = 5f, m_minCrushOffset = 2f, m_crushedTime = 2f;
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private AudioClip m_dabSound, m_whipSound, m_crushSound, m_pickupCoinSound;

        private bool m_grounded = false, m_canMove = true, m_canJump = false;
        private Vector3 m_targetVelocity = Vector3.zero, m_velocity, m_scale;
        private Quaternion m_lookDirection;

        private Pool m_coinPool;

        [SerializeField]
        private int m_money = 0, m_dropCoinAmaount = 3;
        
        public int money { get { return m_money; } }

        [Header("UI")]
        [SerializeField]
        private Text m_scoreText;


        void Start()
        {
            //Save some variables
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            m_coinPool = m_gameManager.coinPool;
            m_scale = transform.localScale;
            m_lookDirection = transform.rotation;
            m_audioSource = gameObject.GetComponent<AudioSource>();

            //Set up rigidbody
            m_rigidbody = gameObject.GetComponent<Rigidbody>();
            m_rigidbody.freezeRotation = true;
            m_rigidbody.useGravity = false;

            //Initialize Controls
            InputManager.Instance.BindAxis("KeenKeld_X_" + m_playerID.ToString(), m_playerID, ControllerAxisCode.LeftStickX);
            InputManager.Instance.BindAxis("KeenKeld_Y_" + m_playerID.ToString(), m_playerID, ControllerAxisCode.LeftStickY);
            InputManager.Instance.BindButton("KeenKeld_Jump_" + m_playerID.ToString(), m_playerID, ControllerButtonCode.A, ButtonState.OnPress);
            InputManager.Instance.BindButton("KeenKeld_Taunt" + m_playerID.ToString(), m_playerID, ControllerButtonCode.X, ButtonState.OnPress);

            //Set player colors
            Renderer[] rs = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                r.material.EnableKeyword("_DETAIL_MULX2");
                r.material.SetTexture("_DetailAlbedoMap", m_gameManager.GenerateTextureFromColor(GlobalGameManager.Instance.GetPlayerColor(m_playerID)));
            }

            m_wallet.material.color = GlobalGameManager.Instance.GetPlayerColor(m_playerID);
        }


        void FixedUpdate()
        {
            if (m_gameManager.gameState == GameState.Playing)
            {
                FixedMovement();
            }
            else
            {
                m_rigidbody.velocity = Vector3.zero;
            }
        }


        void Update()
        {
            if (m_gameManager.gameState == GameState.Playing)
            {
                CheckGameOver();

                CheckGrounded();
                Movement();
                Attacking();
                Taunting();

                Animations();
            }
        }


        void LateUpdate()
        {
            UpdateUI();
        }


        void CheckGrounded()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, m_checkGroundedLength))
            {
                if (hit.transform != transform && hit.transform.parent != transform)
                {
                    m_grounded = true;
                }
            }
            else
            {
                m_grounded = false;
                m_animator.SetBool("attacking", false);
            }

            Debug.DrawRay(transform.position, -transform.up * m_checkGroundedLength);

            if (hit.point != null)
                Debug.DrawLine(transform.position, hit.point, Color.red);
        }


        void Movement()
        {
            //Jumping
            if (m_canMove && m_grounded && m_canJump && InputManager.Instance.GetButton("KeenKeld_Jump_" + m_playerID.ToString()))
            {
                m_rigidbody.velocity = new Vector3(m_velocity.x, CalculateJumpVerticalSpeed(), m_velocity.z);
                m_animator.CrossFade("Jump", 0.2f);
            }

            //Look towards walking position
            if (Mathf.Abs(m_targetVelocity.x) >= 0.1f || Mathf.Abs(m_targetVelocity.z) >= 0.1f)
            {
                m_lookDirection = Quaternion.LookRotation(Vector3.Normalize(m_targetVelocity), Vector3.up);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, m_lookDirection, m_turnSpeed * Time.deltaTime);
        }


        void Attacking()
        {
            //Initiate stomp attack
            if (!m_grounded && m_canJump && InputManager.Instance.GetButton("KeenKeld_Jump_" + m_playerID.ToString()))
            {
                Vector3 newVel = new Vector3();
                newVel.x = 0;
                newVel.y = -m_stompSpeed;
                newVel.z = 0;
                m_rigidbody.velocity = newVel;

                m_animator.SetBool("attacking", true);
            }
        }


        void Taunting ()
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dab") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Whip"))
            {
                m_canMove = true;

                if (m_grounded && m_canMove && InputManager.Instance.GetButton("KeenKeld_Taunt" + m_playerID.ToString()))
                {
                    m_canMove = false;

                    if (Random.Range(0f, 5f) <= 1f)
                    {
                        m_animator.Play("Whip", 0);
                        m_audioSource.PlayOneShot(m_whipSound);
                    }
                    else
                    {
                        m_animator.Play("Dab", 0);
                        m_audioSource.PlayOneShot(m_dabSound);
                    }
                }
            }
            else
            {
                m_canMove = false;
            }
           
        }


        void FixedMovement()
        {
            m_canJump = true;

            if (m_canMove && m_grounded)
            {
                //Walking
                float xMovement = InputManager.Instance.GetAxis("KeenKeld_X_" + m_playerID.ToString());
                float yMovement = InputManager.Instance.GetAxis("KeenKeld_Y_" + m_playerID.ToString());
                m_targetVelocity = new Vector3(xMovement, 0, yMovement);
                m_targetVelocity *= m_moveSpeed;

                //Reach Target Velocity
                m_velocity = m_rigidbody.velocity;
                Vector3 velocityChange = (m_targetVelocity - m_velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -m_maxVelocityChange, m_maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -m_maxVelocityChange, m_maxVelocityChange);
                velocityChange.y = 0;
                m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            //Manual gravity for more control
            m_rigidbody.AddForce(new Vector3(0, -m_gravity * m_rigidbody.mass, 0));
        }


        void Animations()
        {
            m_animator.SetBool("grounded", m_grounded);

            if (m_grounded)
            {
                float speed = (m_velocity / m_moveSpeed).magnitude;
                m_animator.SetFloat("speed", speed);
            }
            else
            {
                m_animator.SetFloat("speed", 0);
            }
        }


        void UpdateUI()
        {
            m_scoreText.text = m_money.ToString();
        }


        float CalculateJumpVerticalSpeed()
        {
            // From the jump height and gravity we deduce the upwards speed for the character to reach at the apex.
            return Mathf.Sqrt(2 * m_jumpHeight * m_gravity);
        }


        public void Crushed()
        {
            //PLAYER GETS CRUSHED
            //print(transform.name + " - player '" + m_playerID + "' got crushed");
            transform.localScale = new Vector3(m_scale.x * 1.2f, .005f, m_scale.z * 1.2f);
            m_canMove = false;
            
            DropCoins();

            m_audioSource.PlayOneShot(m_crushSound);

            Invoke("Uncrush", m_crushedTime);
        }


        void DropCoins ()
        {
            for (int i = 0; i < m_money; i++)
            {
                    Vector3 randomOffset = new Vector3(Random.Range(-3, 3), Random.Range(1, 4), Random.Range(-3, 3));
                    m_coinPool.Instantiate(transform.position + randomOffset, Quaternion.identity);
            }
            m_money = 0;
        }


        void Uncrush ()
        {
            transform.localScale = m_scale;
            m_canMove = true;
        }


        public void GrabbedCoin()
        {
            m_audioSource.PlayOneShot(m_pickupCoinSound);
            m_money++;
        }


        void OnCollisionEnter (Collision collision)
        {
            if (collision.transform.gameObject.tag == "Player" && transform.position.y > collision.transform.position.y + m_minCrushOffset)
            {
                collision.transform.gameObject.GetComponent<Player>().Crushed();
            }
        }

        void CheckGameOver ()
        {
            if (m_money >= m_gameManager.maxPlayerScore)
            {
                m_gameManager.GameOver();
            }
        }
    }
}