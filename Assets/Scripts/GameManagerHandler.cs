using Unity.Scripting.LifecycleManagement;
using UnityEngine;

public class GameManagerHandler : MonoBehaviour
{
    public static bool isGamePaused = false;

    void Start()
    {
        //XPHandler.OnLevelUpdated += HandleLevelUpdated;
    }

    private void OnDisable()
    {
        //XPHandler.OnLevelUpdated -= HandleLevelUpdated;
    }
    void HandleLevelUpdated(int newLevel)
    {
        // Handle level updated logic here
        isGamePaused = true;
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
    }

    public static void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume the game by setting time scale to 1
    }
}
