using UnityEngine;
using TMPro;
using EasyTextEffects;

public class WaveUIHandler : MonoBehaviour
{
    public TMP_Text Text;
    public float displayDuration = 5f;
    public Animator animator;
    private readonly string animationParameter = "Show";
    void Start()
    {
        WaveManager.OnWaveUpdated += ShowWaveInfo;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveUpdated -= ShowWaveInfo;
    }

    public void ShowWaveInfo(int waveNumber, WaveManager.WaveState state)
    {
        Text.GetComponent<TextEffect>().StartOnStartEffects();
        string waveText = $"Wave {waveNumber + 1}";
        switch (state)
        {
            case WaveManager.WaveState.Waiting:
                waveText = "Wait for Next Wave!";
                break;
            case WaveManager.WaveState.Active:
                waveText = $"Wave {waveNumber + 1}!";
                break;
            case WaveManager.WaveState.Cleared:
                waveText = "Wave Complete!";
                break;
            case WaveManager.WaveState.LastWave:
                waveText = "Last Wave!";
                break;
            case WaveManager.WaveState.Finished:
                waveText = "All Waves Complete!";
                break;
        }
        Text.text = waveText;
        animator.SetBool(animationParameter, true);
        Invoke(nameof(HideWaveInfo), displayDuration);
    }

    private void HideWaveInfo()
    {
        animator.SetBool(animationParameter, false);
    }
}


