using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SingleWaveSO;
public class WaveManager : MonoBehaviour
{
    public List<MainWave> waves;
    public GameObject NextWaveButton;
    public enum WaveState
    {
        Waiting,
        Active,
        Cleared,
        Finished,
        LastWave
    }
    private float elapsedTime = 0f;
    private float timer = 0f;
    public static Action<int, WaveState> OnWaveUpdated;
    public static Action OnAllWavesFinished;
    public GameObject waveDirectionIndicator;
    public float changeRegionDistanceThreshold;
    private ShipMovementHandler playerShip;
    private Vector2 lastClearedWave;
    private AudioHandler audioHandler;
    IEnumerator Start()
    {
        audioHandler = GetComponent<AudioHandler>();
        playerShip = FindAnyObjectByType<ShipMovementHandler>();
        yield return new WaitForSeconds(0.1f);
        OnWaveUpdated?.Invoke(0, WaveState.Active);
        audioHandler.PlaySFX(SFXType.Brush);
        GameObject obj = new GameObject("Wave 0");
        obj.transform.parent = this.transform;
        for (int j = 0; j < waves[0].subWaves.Count; j++)
        {
            GameObject subObj = new GameObject("SubWave " + j);
            subObj.transform.parent = obj.transform;
            subObj.AddComponent<SingleWaveContainer>().Initialize(waves[0].subWaves[j], new Vector2(0, j));
            subObj.GetComponent<SingleWaveContainer>().WaveDirectionIndicator = waveDirectionIndicator;
        }
        yield return null;
    }

    public IEnumerator DoSendNextWave(Vector2 waveID)
    {

        bool isLastMainWave = (waves.Count - 1 == waveID.x);
        if (waves[(int)waveID.x].subWaves.Count - 1 == waveID.y)
        {
            if (isLastMainWave)
            {
                OnWaveUpdated?.Invoke((int)waveID.x, WaveState.Finished);
                audioHandler.PlaySFX(SFXType.Brush);
                OnAllWavesFinished?.Invoke();
                yield return null;

            }
            else
            {
                OnWaveUpdated?.Invoke((int)waveID.x + 1, WaveState.Waiting);
                audioHandler.PlaySFX(SFXType.Brush);
                yield return new WaitForSeconds(waves[(int)waveID.x + 1].startTime); // Wait for the next wave's start time
                OnWaveUpdated?.Invoke((int)waveID.x + 1, (waveID.x + 1 == waves.Count - 1) ? WaveState.LastWave : WaveState.Active);
                audioHandler.PlaySFX(SFXType.Brush);
                int nextWaveIndex = (int)waveID.x + 1;

                GameObject obj = new GameObject("Wave " + nextWaveIndex);
                obj.transform.parent = this.transform;

                for (int j = 0; j < waves[nextWaveIndex].subWaves.Count; j++)
                {
                    GameObject subObj = new GameObject("SubWave " + j);
                    subObj.transform.parent = obj.transform;
                    SingleWaveContainer singleWavecontainer = subObj.AddComponent<SingleWaveContainer>();
                    singleWavecontainer.Initialize(waves[nextWaveIndex].subWaves[j], new Vector2(nextWaveIndex, j));
                    singleWavecontainer.WaveDirectionIndicator = waveDirectionIndicator;
                }
            }

        }
        yield return null;
    }

    public void NextWaveButtonClicked()
    {
        NextWaveButton.SetActive(false);
        StartCoroutine(DoSendNextWave(lastClearedWave));
        audioHandler.PlaySFX(SFXType.Select);
    }

    public void SetSubWaveAsCleared(Vector2 waveID)
    {
        lastClearedWave = waveID;
        waves[(int)waveID.x].subWaves[(int)waveID.y].isCleared = true;
        bool isLastMainWave = waves.Count - 1 == waveID.x;
        if (isLastMainWave)
            return;

        if (waves[(int)waveID.x].AreAllSubWavesCleared())
        {
            NextWaveButton.SetActive(true);
        }
    }

}

[Serializable]
public class SubWave
{
    public float startTime;
    public SingleWaveSO singleWaveData;
    public Transform region;
    public Transform alternativeRegion;
    internal bool useAlternativeRegion;
    internal bool isCleared;
}

[Serializable]
public class MainWave
{
    public float startTime;
    public List<SubWave> subWaves;

    public bool AreAllSubWavesCleared()
    {
        foreach (var subWave in subWaves)
        {
            if (!subWave.isCleared)
                return false;
        }
        return true;
    }
}