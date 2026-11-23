using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerHandler : MonoBehaviour
{
    public static GameManagerHandler instance;
    public static bool isGamePaused = false;
    public InputActionReference action;
    public GameObject PauseMenu;
    public static Action OnGamePaused;
    public static Action OnGameResumed;

    private bool isAnyOverlayScreenActive;
    private void Awake()
    {
        instance = this;
        

    }
    void Start()
    {
        ResumeGame();
        action.action.performed += HandleInput;
        PlayerHPHandler.OnDie += PauseGame;
        OnGamePaused += PauseGame;
        OnGameResumed += ResumeGame;
        ResultMenu.OnResultScreenShown += SetIsAnyOverlayScreenActive;
    }

    public void HandleInput(InputAction.CallbackContext ctx)
    {

        if (isGamePaused)
        {
            OnGameResumed?.Invoke();
        }
        else
            OnGamePaused?.Invoke();
    }
    private void OnDisable()
    {
        action.action.performed -= HandleInput;
        OnGamePaused -= PauseGame;
        OnGameResumed -= ResumeGame;
        PlayerHPHandler.OnDie -= PauseGame;
        ResultMenu.OnResultScreenShown -= SetIsAnyOverlayScreenActive;
    }
    void HandleLevelUpdated(int newLevel)
    {
        isGamePaused = true;
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }
    public static void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public static void QuitGame()
    {
        Application.Quit();
        
    }

    public void SetIsAnyOverlayScreenActive()
    {
        isAnyOverlayScreenActive = true;
    }

    public static bool GetIsAnyOverlayScreenActive()
    {
        return instance.isAnyOverlayScreenActive;
    }

}
