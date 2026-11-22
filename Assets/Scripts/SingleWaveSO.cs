using UnityEngine;

[CreateAssetMenu(fileName = "SingleWaveSO", menuName = "Scriptable Objects/SingleWaveSO", order = 1)]
public class SingleWaveSO : ScriptableObject
{
    public Wave wave;
    [System.Serializable]
    public struct Wave
    {
        //public float spawnRate;
        public int enemyCount;
        public GameObject enemyPrefab;
    }
}
