using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pawnPrefab;
    private List<Pawn> _pawns;
    private int _currentlyMoving = 0;

    [SerializeField]
    private BoardManager _boardManager;

    private void Awake()
    {
        GlobalGameManager.Instance.GameStartEvent += OnGameStart;
    }

    private void OnDestroy()
    {
        GlobalGameManager.Instance.GameStartEvent -= OnGameStart;
    }

    private void OnGameStart()
    {
        InitializePawns();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (GlobalGameManager.Instance.GetGameState() != GameState.STATE_BOARD)
            return;

        if (level == GlobalGameManager.Instance.GetBoardLevelID())
        {
            InitializePawns();
            StartMoving();
        }
    }

    private void InitializePawns()
    {
        if (_pawns != null) _pawns.Clear();
        else _pawns = new List<Pawn>();

        GlobalGameManager gameManager = GlobalGameManager.Instance;

        int playerCount = gameManager.GetPlayerCount();
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
                pawn.SetCurrentNode(currentNode);
                pawn.SetColor(gameManager.GetPlayerColor(i));
            }
            else
            {
                Debug.LogWarning("The pawn prefab doesn't contain the pawn script!");
                return;
            }
        }
    }

    private void StartMoving()
    {
        _currentlyMoving = 0;
        MovePawn();
    }

    private void MovePawn()
    {
        if (_currentlyMoving >= _pawns.Count)
        {
            GlobalGameManager.Instance.OnAllPawnsMoved();
            return;
        }

        int moveCount = GlobalGameManager.Instance.GetAddedPawnPosition(_currentlyMoving);
        _pawns[_currentlyMoving].Move(moveCount, OnPawnMoved);
    }

    private void OnPawnMoved()
    {
        _currentlyMoving += 1;
        MovePawn();
    }
}
