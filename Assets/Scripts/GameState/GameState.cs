using System;
using UnityEngine;

public static class GameState
{
    public static bool GameFinished = false;
    public static event Action OnGameFinished;

    public static void FinishGame()
    {
        if (GameFinished) return; // prevent multiple invocations
        GameFinished = true;
        OnGameFinished?.Invoke();
    }
    
    public static void GameStart()
    {
        GameFinished = false;
    }
}
