using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XBOXParty;

namespace Board
{
    public class PawnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pawnPrefab;
        private List<Pawn> _pawns;
        private int _numMoved = 0;
        private List<int> _orderOfMovement;

        [SerializeField]
        private Vector2 _offset;

        [SerializeField]
        private BoardManager _boardManager;

        private void Start()
        {
            GlobalGameManager.Instance.GameStartEvent += OnGameStart;
            GlobalGameManager.Instance.ResetGameEvent += OnGameReset;
        }

        private void OnDestroy()
        {
            if (GlobalGameManager.Instance != null)
            {
                GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
                GlobalGameManager.Instance.ResetGameEvent -= OnGameReset;
            }
        }

        private void OnGameStart()
        {
            InitializePawns();
        }

        private void OnGameReset()
        {
            ClearPawns();
        }

        private void OnLevelWasLoaded(int level)
        {
            if (GlobalGameManager.Instance.GameState != GameState.STATE_BOARD)
                return;

            if (level == GlobalGameManager.Instance.BoardLevelID)
            {
                InitializePawns();
                StartMoving();
            }
        }

        private void InitializePawns()
        {
            if (_pawns != null)
            {
                ClearPawns();
            }
            else
            {
                _pawns = new List<Pawn>();
            }

            GlobalGameManager gameManager = GlobalGameManager.Instance;

            int playerCount = gameManager.PlayerCount;
            if (playerCount <= 0)
                return;

            for (int i = 0; i < playerCount; ++i)
            {
                GameObject go = GameObject.Instantiate(_pawnPrefab) as GameObject;
                Pawn pawn = go.GetComponent<Pawn>();

                if (pawn != null)
                {
                    _pawns.Add(pawn);
                    Node currentNode = _boardManager.GetNode(gameManager.GetCurrentPawnPosition(i));
                    //Determine offset
                    Vector2 offset = _offset;
                    if (i % 2 == 0) offset.x *= -1.0f;

                    if (playerCount <= 2) offset.y = 0.0f;
                    else if (i >= 2) offset.y *= -1.0f;

                    pawn.SetOffset(offset);
                    pawn.SetColor(gameManager.GetPlayerColor(i));
                    pawn.SetOrderInLayer(-playerCount + i);

                    pawn.SetCurrentNode(currentNode);
                }
                else
                {
                    Debug.LogWarning("The pawn prefab doesn't contain the pawn script!");
                    return;
                }
            }
        }

        private void ClearPawns()
        {
            for (int i = 0; i < _pawns.Count; ++i)
            {
                GameObject.Destroy(_pawns[i].gameObject);
            }
            _pawns.Clear();
        }

        private void StartMoving()
        {
            if (_orderOfMovement != null)
                _orderOfMovement.Clear();
            else
                _orderOfMovement = new List<int>();

            _numMoved = 0;

            _orderOfMovement = SortPawns(true);
            MovePawn();
        }

        private void MovePawn()
        {
            if (_numMoved >= _pawns.Count)
            {
                OnAllPawnsMoved();
                return;
            }

            int currentlyMoving = _orderOfMovement[_numMoved];

            int moveCount = GlobalGameManager.Instance.GetAddedPawnPosition(currentlyMoving);
            _pawns[currentlyMoving].Move(moveCount, OnPawnMoved);
        }

        private void OnPawnMoved()
        {
            _numMoved += 1;
            MovePawn();
        }

        private void OnAllPawnsMoved()
        {
            //Check if any pawn has reached their final destination
            for (int i = 0; i < _pawns.Count; ++i)
            {
                if (_pawns[i].IsOnLastNode())
                {
                    OnFinish();
                    break;
                }
            }

            GlobalGameManager.Instance.OnAllPawnsMoved();
        }

        private void OnFinish()
        {
            //At least 1 pawn has finished, determine the positions
            GlobalGameManager.Instance.PlayerFinished(SortPawns(false));
        }

        private List<int> SortPawns(bool sortOnAddedPositions)
        {
            //Keep out please, even I have to deal with deadlines :)
            List<KeyValuePair<int, int>> tempList = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < _pawns.Count; ++i)
            {
                int nodeID = 0;

                if (sortOnAddedPositions)
                    nodeID = GlobalGameManager.Instance.GetAddedPawnPosition(i);
                else
                    nodeID = _boardManager.GetNodeId(_pawns[i].CurrentNode);

                tempList.Add(new KeyValuePair<int, int>(i, nodeID));
            }
            tempList.Sort((x, y) => -1 * x.Value.CompareTo(y.Value));

            //Put them in a useful format (places can be shared!)
            List<int> resultList = new List<int>();
            for (int i = 0; i < _pawns.Count; ++i)
            {
                resultList.Add(0);
            }

            int currentPos = -1;
            for (int i = 0; i < tempList.Count; ++i)
            {
                if (i == 0 || tempList[i].Value != tempList[i - 1].Value)
                    currentPos += 1;

                if (sortOnAddedPositions)
                    resultList[i] = tempList[i].Key;
                else
                    resultList[tempList[i].Key] = currentPos;
            }

            return resultList;
        }
    }
}