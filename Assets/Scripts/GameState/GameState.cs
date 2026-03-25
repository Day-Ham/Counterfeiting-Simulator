using System;
using UnityEngine;

public static class GameState
{
    public static bool IsGameFinished = false;
    public static bool IsGamePaused = false;

    public static event Action OnGameStarted;
    public static event Action OnGameFinished;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    public static void GameStart()
    {
        IsGameFinished = false;
        IsGamePaused = false;

        OnGameStarted?.Invoke();
    }

    public static void FinishGame()
    {
        if (IsGameFinished) return;

        IsGameFinished = true;
        IsGamePaused = false;

        OnGameFinished?.Invoke();
    }

    public static void PauseGame()
    {
        if (IsGamePaused || IsGameFinished) return;

        IsGamePaused = true;
        OnGamePaused?.Invoke();
    }

    public static void ResumeGame()
    {
        if (!IsGamePaused || IsGameFinished) return;

        IsGamePaused = false;
        OnGameResumed?.Invoke();
    }
}
