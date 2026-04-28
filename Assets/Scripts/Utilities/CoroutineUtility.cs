using UnityEngine;
using System.Collections;

public static class CoroutineUtility
{
    // A wait that is aware of the game's paused state. It will only count down when the game is not paused.
    public static IEnumerator PauseAwareWait(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            if (!GameState.IsGamePaused)
                elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
