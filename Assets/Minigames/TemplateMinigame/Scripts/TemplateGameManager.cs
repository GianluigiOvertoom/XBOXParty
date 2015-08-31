using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemplateGameManager : MonoBehaviour
{
    private List<int> _positions;
    private int _selectedPlayer;

    private void Awake()
    {
        _positions = new List<int>();

        int playerCount = GlobalGameManager.Instance.GetPlayerCount();
        for (int i = 0; i < playerCount; ++i)
        {
            _positions.Add(0);
        }
    }

    public void SelectPlayer(int id)
    {
        _selectedPlayer = id;
        Debug.Log("Selected player: " + _selectedPlayer);
    }

    public void SetPlayerPosition(int position)
    {
        _positions[_selectedPlayer] = position;
        string str = "Current rankings: ";

        for (int i = 0; i < _positions.Count; ++i)
        {
            str += "P" + i + ": " + _positions[i] + " / ";
        }

        Debug.Log(str);
    }

    public void Submit()
    {
        GlobalGameManager.Instance.SubmitGameResults(_positions);
    }
}
