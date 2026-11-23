using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private string winString;
    [SerializeField] private string loseString;
    [SerializeField] private GameObject winButtons;
    [SerializeField] private GameObject loseButtons;

    public static Action OnResultScreenShown;
    private void Start()
    {
        PlayerHPHandler.OnDie += DisplayLoseResults ;
        WaveManager.OnAllWavesFinished += DisplayWinResults;
    }

    private void OnDisable()
    {
        PlayerHPHandler.OnDie -= DisplayLoseResults;
        WaveManager.OnAllWavesFinished -= DisplayWinResults;
    }

    private void DisplayLoseResults()
    {
        resultText.text = loseString;
        winButtons.SetActive(false);
        loseButtons.SetActive(true);
        visual.SetActive(true);
        OnResultScreenShown?.Invoke();
    }

    private void DisplayWinResults()
    {
        resultText.text = winString;
        winButtons.SetActive(true);
        loseButtons.SetActive(false);
        visual.SetActive(true);
        OnResultScreenShown?.Invoke();
    }
    public void OnContinueButtonClicked()
    {
        GameManagerHandler.ResumeGame();
        visual.SetActive(false);
        
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
