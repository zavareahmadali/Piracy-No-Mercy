using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerHandler : MonoBehaviour
{
    public static GameManagerHandler instance;
    public static bool isGamePaused = false;
    public InputActionReference action;
    public GameObject PauseMenu;

    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        action.action.performed += HandleInput;
    }

    public void HandleInput(InputAction.CallbackContext ctx)
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
            PauseGame();
    }
    private void OnDisable()
    {
        action.action.performed -= HandleInput;
    }
    void HandleLevelUpdated(int newLevel)
    {
        isGamePaused = true;
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        instance.PauseMenu.SetActive(true);
    }
    public static void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        instance.PauseMenu.SetActive(false);
    }

    public static void QuitGame()
    {
        Application.Quit();
        
    }

}
