using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XBOXParty;

namespace KeenKeld
{
    public enum GameState { PreGame, Playing, PostGame }

    public class GameManager : MonoBehaviour
    {
        //private GlobalGameManager m_globalGameManager;
        //public GlobalGameManager globalGameManager { get { return m_globalGameManager; } }

        [SerializeField]
        private List<Transform> m_players;
        public List<Transform> players { get { return m_players; } }

        [SerializeField]
        private GameState m_gameState;
        public GameState gameState { get { return m_gameState; } }

        [SerializeField]
        private int m_maxCoins = 100, m_maxPlayerScore = 20;
        public int maxPlayerScore { get { return m_maxPlayerScore; } }
        [SerializeField]
        private Pool m_coinPool;
        public Pool coinPool { get { return m_coinPool; } }
        [SerializeField]
        private BoxCollider m_coinSpawnZone;
        [SerializeField]
        private Vector2 m_minMaxSpawnTime = new Vector2(1f, 4f);
        private float m_timer, m_randomTime;
        private int m_spawnedCoins = 0;

        //UI
        [Header("UI")]
        [SerializeField]
        private GameObject m_pauseScreen;
        [SerializeField]
        private GameObject[] m_playerScores;
        [SerializeField]
        private Image m_preGameScreen, m_postGameScreen;
        [SerializeField]
        private Text m_postGameText;
        [SerializeField]
        private float m_fadePreAfter = 3f, m_fadeTime = 2f;


        void Awake()
        {
            for (int i = 0; i < GlobalGameManager.Instance.PlayerCount; i++)
            {
                if (m_players[i] != null)
                {
                    m_players[i].gameObject.SetActive(true);
                    m_playerScores[i].SetActive(true);
                }
            }

            //Bind pause button
            InputManager.Instance.BindButton("KeenKeld_Pause_0", 0, ControllerButtonCode.Start, ButtonState.OnPress);
        }

        void Start ()
        {
            if(m_pauseScreen == null)
                m_pauseScreen = GameObject.Find("PauseScreen");

            m_pauseScreen.SetActive(false);

            SetRandomCoinTime();

            if(m_gameState == GameState.PreGame)
            {
                StartCoroutine(PreGame());
            }
        }

        IEnumerator PreGame ()
        {
            if (!m_preGameScreen.gameObject.activeSelf)
                m_preGameScreen.gameObject.SetActive(true);

            yield return new WaitForSeconds(m_fadePreAfter);
            StartCoroutine(LerpUIColor(m_preGameScreen, 1f, 0f, m_fadeTime));
            yield return new WaitForSeconds(m_fadeTime);
            m_gameState = GameState.Playing;
        }

        void Update()
        {
            //Pause
            CheckInput();

            //SpawnCoins
            SpawnCoins();
        }

        void CheckInput ()
        {
            if (InputManager.Instance.GetButton("KeenKeld_Pause_0"))
            {
                if (Time.timeScale == 0f)
                {
                    m_pauseScreen.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    m_pauseScreen.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }

        void SpawnCoins ()
        {
            if (m_gameState == GameState.Playing)
            {
                if (m_spawnedCoins < m_maxCoins)
                {
                    m_timer += Time.deltaTime;

                    if (m_timer > m_randomTime)
                    {
                        m_timer = 0f;
                        m_spawnedCoins++;
                        m_coinPool.Instantiate(GetRandomPointInBox(), Quaternion.identity);
                        SetRandomCoinTime();
                    }
                }
                else
                {
                    GameOver();
                }
            }
        }

        public void GameOver ()
        {
            m_gameState = GameState.PostGame;
            StartCoroutine(PostGame());
        }

        IEnumerator PostGame ()
        {
            //  fill lists to submit and get player with highest score
            List<int> scores = new List<int>();
            List<int> positions = new List<int>();
            int first = 0;
            int highest = 0;

            for (int i = 0; i < GlobalGameManager.Instance.PlayerCount; i++)
            {
                Player p = m_players[i].gameObject.GetComponent<Player>();

                scores.Add(p.money);

                if(p.money > highest)
                {
                    highest = p.money;
                    first = i;
                }
            }

            PrintIntList(scores, "Scores");
            positions = SortPlayerPositions(scores);

            //  show ending and winning player
            m_postGameScreen.gameObject.SetActive(true);
            m_postGameText.gameObject.SetActive(true);

            m_postGameText.text = "Player " + (positions[first] + 1) + " wins!";

            LerpUIColor(m_postGameScreen, 0f, 1f, 2f);
            LerpUIColor(m_postGameText, 0f, 1f, 2f);

            yield return new WaitForSeconds(3);

            Submit(positions);
        }

        List<int> SortPlayerPositions (List<int> playerScores)
        {
            List<int> scores = new List<int>(playerScores); //   EX (8, 15, 20, 1)
            List<int> positions = new List<int>();

            //sort
            scores.Sort();
            PrintIntList(scores, "Sorted"); //  EX (1, 8, 15, 20)
            scores.Reverse();
            PrintIntList(scores, "Reversed"); //  EX (20, 15, 8, 1)

            for (int i  = 0; i  < playerScores.Count; i++)
            {
                for (int j = 0; j < scores.Count; j++)
                {
                    if(scores[j] == playerScores[i])
                    {
                        positions.Add(j);
                        break;
                    }
                }
            }
            PrintIntList(positions, "Positions");
            return positions;
        }

        public void Submit(List<int> positions)
        {
            GlobalGameManager.Instance.SubmitGameResults(positions);
        }

        void SetRandomCoinTime ()
        {
            m_randomTime = Random.Range(m_minMaxSpawnTime.x, m_minMaxSpawnTime.y);
        }

        private IEnumerator LerpUIColor (MaskableGraphic graphic, float alphaStart, float alphaFinish, float time)
        {
            float elapsedTime = 0;
            Color startColor = graphic.color, endColor = graphic.color;
            startColor.a = alphaStart;
            endColor.a = alphaFinish;

            graphic.color = startColor;

            while (elapsedTime < time)
            {
                graphic.color = Color.Lerp(startColor, endColor, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return 0;
            }
            graphic.color = endColor;
        }

        Vector3 GetRandomPointInBox ()
        {
            float x = Random.Range(m_coinSpawnZone.bounds.min.x, m_coinSpawnZone.bounds.max.x);
            float y = Random.Range(m_coinSpawnZone.bounds.min.y, m_coinSpawnZone.bounds.max.y);
            float z = Random.Range(m_coinSpawnZone.bounds.min.z, m_coinSpawnZone.bounds.max.z);

            return new Vector3(x, y, z);
        }

        public Texture2D GenerateTextureFromColor (Color color)
        {
            Texture2D tex = new Texture2D(64, 64);

            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    tex.SetPixel(i, j, color);
                }
            }

            tex.Apply();

            return tex;
        }

        public void PrintIntList (List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                print("List[" + i + "] = " + list[i]);
            }
        }

        public void PrintIntList(List<int> list, string debug)
        {
            for (int i = 0; i < list.Count; i++)
            {
                print(debug + "; List[" + i + "] = " + list[i]);
            }
        }
    }
}