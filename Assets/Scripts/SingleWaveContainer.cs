using UnityEngine;

public class SingleWaveContainer : MonoBehaviour
{

    public SubWave subWave;
    public GameObject WaveDirectionIndicator;
    private Vector2 waveIndex;
    private float elapsedTime = 0f;
    private Vector3 tempPos;
    private bool hasSpawned = false;
    private int remainingEnemies;
    private ShipMovementHandler playership;
    private void Start()
    {
        playership = FindAnyObjectByType<ShipMovementHandler>();
        hasSpawned = false;
        remainingEnemies = subWave.singleWaveData.wave.enemyCount;
    }
    

    void FixedUpdate()
    {
        if (hasSpawned)
            return;

        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime < subWave.startTime)
            return;


        if (elapsedTime >= subWave.startTime)
        {
            for (int i = 0; i < subWave.singleWaveData.wave.enemyCount; i++)
            {
                if (Vector3.Distance(subWave.region.position, playership.transform.position) > 75)
                    tempPos = subWave.region.position + UnityEngine.Random.insideUnitSphere * subWave.region.localScale.x;
                else
                {
                    if (subWave.alternativeRegion != null)
                        tempPos = subWave.alternativeRegion.position + UnityEngine.Random.insideUnitSphere * subWave.region.localScale.x;
                    else
                        Debug.Log("Too close to player but alternative region not set!");
                }
                tempPos.y = 0; // Keep the y position at 0 to spawn on the ground
                AIShipBase ship = Instantiate(subWave.singleWaveData.wave.enemyPrefab, tempPos, Quaternion.identity).GetComponent<AIShipBase>();
                ship.GetComponent<HPHandler>().OnDie += OnShipDie;
            }
            hasSpawned = true;
            Instantiate(WaveDirectionIndicator, tempPos, Quaternion.identity).GetComponent<WaveDirectionIndicator>().Initialize(tempPos, FindAnyObjectByType<ShipMovementHandler>().transform);
        }
    }
    public void Initialize(SubWave subWave, Vector2 waveID)
    {
        this.subWave = subWave;
        this.waveIndex = waveID;
    }

    public void OnShipDie()
    {
        
        remainingEnemies--;
        if (remainingEnemies <= 0)
        {
            FindAnyObjectByType<WaveManager>().SetSubWaveAsCleared(waveIndex);
        }
    }
}
