using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    
    void Start()
    {
        GameManagerHandler.OnGamePaused += OnPause;
        GameManagerHandler.OnGameResumed += OnResume;
    }

    private void OnDisable()
    {
        GameManagerHandler.OnGamePaused -= OnPause;
        GameManagerHandler.OnGameResumed -= OnResume;
    }
    private void OnPause()
    {
        visual.SetActive(true);
    }
    private void OnResume()
    {
        visual.SetActive(false);
    }
}
