using UnityEngine;
using System.Collections;
using XBOXParty;

namespace SquareSiege
{
    public class TankController : MonoBehaviour
    {
        private GlobalGameManager m_GlobalGameManager;

        private GameController m_GameController;

        #region Movement&Collision Vars
        [SerializeField]
        private LayerMask m_CollisionLayer = 8;

        [SerializeField]
        private Vector3 m_Velocity;

        [SerializeField]
        private float
            m_Gravity = -.8f,
            m_MovementSpeed = 2f,
            //m_JumpingSpeed = .25f,
            m_TurretRotatingSpeed = 3,
            m_Indenting = .05f;

        private BoxCollider m_Collider;
        private Bounds m_Bounds;

        private Vector3
            m_LeftUpperLeft,
            m_RightUpperLeft,
            m_BottomUpperLeft,
            m_TopUpperLeft,
            m_FrontUpperLeft,
            m_BackUpperLeft;

        //private Vector3 m_OldDirection;

        private int
            m_LengthDivision = 32,
            m_RayCount = 5;

        private bool m_Grounded;

        [SerializeField]
        private Transform m_TankBase, m_TankTurret;
        #endregion

        #region Combat Vars
        [SerializeField]
        private int m_AttackDamage = 1;
        public int AttackDamage { get { return m_AttackDamage; } }

        [SerializeField]
        private GameObject m_SecondBarrel;

        [SerializeField]
        private float m_ShootingCooldown = 2.0f, m_ShootingTimer = .0f;

        [SerializeField]
        private Transform m_ShootingPoint;

        private bool m_IsAlive = true;
        public bool IsAlive { get { return m_IsAlive; } }
        #endregion

        #region Sound Vars
        private SoundController m_SoundController;

        [SerializeField]
        private AudioClip
            m_SoundShooting,
            m_SoundExplosion,
            m_SoundJam,
            m_SoundTankHit;
        #endregion

        #region Misc Vars
        private bool m_Falling = false;

        private float m_WinDelayTimer = 8.0f;

        private int m_BasePlayer = -1;
        public int BasePlayer { get { return m_BasePlayer; } }

        private int m_TurretPlayer = -1;
        public int TurretPlayer { get { return m_TurretPlayer; } }

        // SFX
        [SerializeField]
        private GameObject m_FireworkEffect, m_HitEffect, m_ExplosionEffect, m_SmokeEffect, m_ShotEffect, m_SteamEffect, m_HitExplosionEffect;

        private TeamController m_TeamController;
        #endregion

        void Awake()
        {
            m_GlobalGameManager = GlobalGameManager.Instance;

            m_GameController = FindObjectOfType<GameController>();

            m_SoundController = GetComponent<SoundController>();

            m_TeamController = transform.parent.GetComponent<TeamController>();
        }

        // Use this for initialization
        void Start()
        {
            m_Collider = GetComponent<BoxCollider>();

            m_Bounds = m_Collider.bounds;
        }

        // Update is called once per frame
        void Update()
        {
            #region Movement&Collision
            UpdateBounds();

            if (m_IsAlive)
            {
                if (m_BasePlayer != -1) ApplyMovement();
            }
            else m_Velocity = Vector3.zero;

            ApplyGravity();

            CheckCollision(m_LengthDivision, m_RayCount);

            //if (Input.GetKey(KeyCode.Space) && m_grounded)
            //{
            //    Jump(m_jumpingSpeed);
            //}

            transform.Translate(m_Velocity * m_MovementSpeed * Time.deltaTime);

            if (m_IsAlive)
            {
                ApplyBaseRotation();
                ApplyTurretRotation();
            }
            #endregion

            #region Combat
            if (m_ShootingTimer > .0f) m_ShootingTimer -= Time.deltaTime;

            if (m_ShootingTimer < .0f) m_ShootingTimer = .0f;

            if (InputManager.Instance.GetButton("SquareSiege_Shoot" + m_TurretPlayer))
            {
                if (m_ShootingTimer == .0f)
                {
                    Shoot();
                }
                else
                {
                    m_SoundController.PlaySound(m_SoundJam);
                }
            }
            #endregion

            if (!m_IsAlive)
            {
                if (m_WinDelayTimer > .0f) m_WinDelayTimer -= Time.deltaTime;
                else m_TeamController.Lose();
            }
        }

        #region Combat Functions
        private void Shoot()
        {
            Debug.DrawRay(m_SecondBarrel.transform.position, -m_SecondBarrel.transform.right * 1000, Color.red, m_ShootingCooldown / 2);

            RaycastHit hit;
            if (Physics.Raycast(m_SecondBarrel.transform.position, -m_SecondBarrel.transform.right, out hit, 1000, m_CollisionLayer))
            {
                if (hit.transform.tag == "Player")
                {
                    TeamController tc = hit.transform.parent.GetComponent<TeamController>();

                    tc.InflictDamage(1);

                    m_SoundController.PlaySound(m_SoundTankHit);
                }

                // hit explosion
                GameObject hitEffect = (GameObject)Instantiate(m_HitExplosionEffect, hit.point, Quaternion.identity);
            }

            ControllerInput.SetVibration(m_BasePlayer, .8f, .8f, .2f);
            ControllerInput.SetVibration(m_TurretPlayer, .8f, .8f, .2f);

            // flare
            GameObject flare = (GameObject)Instantiate(m_ShotEffect, m_ShootingPoint.position, Quaternion.identity);
            flare.transform.SetParent(m_TankTurret.transform);

            // steam
            GameObject steam = (GameObject)Instantiate(m_SteamEffect, m_ShootingPoint.position, Quaternion.identity);
            steam.transform.SetParent(m_TankTurret.transform);

            // sound
            m_SoundController.PlaySound(m_SoundShooting, 0);

            m_ShootingTimer = m_ShootingCooldown;
        }

        public void Die()
        {
            if (m_IsAlive)
            {
                // explosion
                GameObject explosion = (GameObject)Instantiate(m_ExplosionEffect, transform.position, transform.rotation);

                // sound
                m_SoundController.PlaySound(m_SoundExplosion);

                // top flies off
                m_TankTurret.GetChild(0).GetComponent<MeshCollider>().enabled = true;
                Rigidbody rb = m_TankTurret.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddExplosionForce(1f, m_TankTurret.transform.position + new Vector3(Random.Range(-2f, 2f), .0f, Random.Range(-2f, 2f)), 1f);

                // smoke
                GameObject smoke = (GameObject)Instantiate(m_SmokeEffect, transform.position, transform.rotation);

                m_IsAlive = false;

                // Fireworks
                Transform t = (m_TeamController.name == m_GameController.Team1.name) ? m_GameController.Team2.Tank.transform : m_GameController.Team1.Tank.transform;
                GameObject fw = Instantiate(m_FireworkEffect);
                fw.transform.position = t.position;
                fw.transform.SetParent(t);
            }
        }
        #endregion

        #region Movement&Collision Functions
        /// <summary>
        /// Continuously checks and applies collision all around the gameObject.
        /// </summary>
        /// <param name="lengthDivision">By how many times the length of the raycasts will be divided.</param>
        /// <param name="rayCount">How many rays squared will be cast per face.</param>
        private void CheckCollision( int lengthDivision, int rayCount )
        {
            float yDirection = Mathf.Sign(m_Velocity.y);
            float xDirection = Mathf.Sign(m_Velocity.x);
            float zDirection = Mathf.Sign(m_Velocity.z);
            float yRayLength = Mathf.Abs(m_Velocity.y / lengthDivision) + m_Indenting;
            float xRayLength = Mathf.Abs(m_Velocity.x / lengthDivision) + m_Indenting;
            float zRayLength = Mathf.Abs(m_Velocity.z / lengthDivision) + m_Indenting;

            float xInterval = m_Bounds.size.x / (rayCount - 1);
            float yInterval = m_Bounds.size.y / (rayCount - 1);
            float zInterval = m_Bounds.size.z / (rayCount - 1);

            m_Grounded = false;

            for (int i = 0; i < rayCount; i++)
            {
                for (int j = 0; j < rayCount; j++)
                {
                    Vector3 xRayOrigin = ((xDirection == -1) ? m_LeftUpperLeft : m_RightUpperLeft);
                    Vector3 yRayOrigin = ((yDirection == -1) ? m_BottomUpperLeft : m_TopUpperLeft);
                    Vector3 zRayOrigin = ((zDirection == -1) ? m_FrontUpperLeft : m_BackUpperLeft);

                    RaycastHit yHit, xHit, zHit;

                    Debug.DrawRay(yRayOrigin + ((Vector3.back * i) * zInterval) + ((Vector3.right * j) * xInterval), transform.up * (m_Velocity.y / lengthDivision), Color.red);
                    Debug.DrawRay(xRayOrigin + ((Vector3.down * i) * yInterval) + (((Vector3.forward * xDirection) * j) * zInterval), transform.right * (m_Velocity.x / lengthDivision), Color.red);
                    Debug.DrawRay(zRayOrigin + ((Vector3.down * i) * yInterval) + ((Vector3.right * j) * xInterval), transform.forward * (m_Velocity.z / lengthDivision), Color.red);

                    // Y axis
                    if (Physics.Raycast(yRayOrigin + ((Vector3.back * i) * zInterval) + ((Vector3.right * j) * xInterval), Vector3.up * yDirection, out yHit, yRayLength, m_CollisionLayer))
                    {
                        m_Velocity.y = (yHit.distance - m_Indenting) * yDirection;
                        yRayLength = yHit.distance;
                        if (yDirection == -1) m_Grounded = true;
                    }

                    // X axis
                    if (Physics.Raycast(xRayOrigin + ((Vector3.down * i) * yInterval) + (((Vector3.forward * xDirection) * j) * zInterval), Vector3.right * xDirection, out xHit, xRayLength, m_CollisionLayer))
                    {
                        m_Velocity.x = (xHit.distance - m_Indenting) * xDirection;
                        xRayLength = xHit.distance;
                    }

                    // Z axis
                    if (Physics.Raycast(zRayOrigin + ((Vector3.down * i) * yInterval) + ((Vector3.right * j) * xInterval), Vector3.forward * zDirection, out zHit, zRayLength, m_CollisionLayer))
                    {
                        m_Velocity.z = (zHit.distance - m_Indenting) * zDirection;
                        zRayLength = zHit.distance;
                    }
                }
            }
        }

        /// <summary>
        /// Update the collision boundaries.
        /// </summary>
        private void UpdateBounds()
        {
            m_Bounds = m_Collider.bounds;

            m_Bounds.Expand(m_Indenting * -2f);

            float minX = m_Bounds.min.x;
            float maxX = m_Bounds.max.x;
            float minY = m_Bounds.min.y;
            float maxY = m_Bounds.max.y;
            float minZ = m_Bounds.min.z;
            float maxZ = m_Bounds.max.z;

            m_BottomUpperLeft = new Vector3(minX, minY, maxZ);
            m_TopUpperLeft = new Vector3(minX, maxY, maxZ);
            m_FrontUpperLeft = new Vector3(minX, maxY, minZ);
            m_BackUpperLeft = new Vector3(minX, maxY, maxZ);
            m_RightUpperLeft = new Vector3(maxX, maxY, minZ);
            m_LeftUpperLeft = m_TopUpperLeft;
        }

        /// <summary>
        /// Apply the movement directed by the player.
        /// </summary>
        private void ApplyMovement()
        {
            float x = .0f;
            float z = .0f;

            if (m_GlobalGameManager.PlayerCount > 0)
            {
                x = -InputManager.Instance.GetAxis("SquareSiege_LH" + m_BasePlayer);
                z = -InputManager.Instance.GetAxis("SquareSiege_LV" + m_BasePlayer);
            }
            else
            {
                x = Input.GetAxis("Horizontal");
                z = Input.GetAxis("Vertical");
            }

            m_Velocity.x = -z;
            m_Velocity.z = x;
        }

        //private void Jump( float velocity )
        //{
        //    m_velocity.y = velocity;
        //}

        private void ApplyBaseRotation()
        {
            Vector3 dir = new Vector3(m_Velocity.x, .0f, m_Velocity.z);

            if (dir.magnitude > .1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);

                //m_TankBase.rotation = Quaternion.Lerp(m_TankBase.rotation, targetRot, Time.deltaTime * 6);
                m_TankBase.rotation = Quaternion.Euler(.0f, targetRot.eulerAngles.y, .0f);

                //m_OldDirection = dir;
            }
        }

        private void ApplyTurretRotation()
        {
            float x = .0f;
            float z = .0f;

            x = -InputManager.Instance.GetAxis(((m_GameController.Dev) ? "SquareSiege_RH" : "SquareSiege_LH") + m_TurretPlayer);
            z = -InputManager.Instance.GetAxis(((m_GameController.Dev) ? "SquareSiege_RV" : "SquareSiege_LV") + m_TurretPlayer);

            Vector3 dir = new Vector3(x, .0f, z);

            if (dir.magnitude > .1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);

                m_TankTurret.rotation = Quaternion.Lerp(m_TankTurret.rotation, targetRot, Time.deltaTime * m_TurretRotatingSpeed);
            }
        }

        private void ApplyGravity()
        {
            m_Velocity.y += m_Gravity * Time.deltaTime;
        }
        #endregion

        public void SetPlayerPositions( int basePlayerID, int turretPlayerID )
        {
            m_BasePlayer = basePlayerID;
            m_TurretPlayer = turretPlayerID;
        }
    }
}
