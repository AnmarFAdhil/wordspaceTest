using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int enemiesPerWave = 3;
    
    private int currentWave = 0;
    private float timeSinceLastWave = 0f;
    private bool bossShouldSpawn = false;
    
    public delegate void WaveChangedDelegate(int waveNumber);
    public event WaveChangedDelegate OnWaveChanged;

    private void Update()
    {
        timeSinceLastWave += Time.deltaTime;
        
        if (timeSinceLastWave >= timeBetweenWaves)
        {
            SpawnWave();
            timeSinceLastWave = 0f;
        }
    }

    private void SpawnWave()
    {
        currentWave++;
        
        bool shouldSpawnBoss = (currentWave % 5 == 0);
        
        if (shouldSpawnBoss && bossPrefab != null)
        {
            Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            int enemiesToSpawn = enemiesPerWave + (currentWave / 3);
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (enemyPrefab != null)
                {
                    Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-1f, 1f));
                    Instantiate(enemyPrefab, spawnPoint.position + spawnOffset, Quaternion.identity);
                }
            }
        }
        
        OnWaveChanged?.Invoke(currentWave);
    }

    public int GetCurrentWave() => currentWave;
    public void SetEnemyPrefab(GameObject prefab) => enemyPrefab = prefab;
    public void SetBossPrefab(GameObject prefab) => bossPrefab = prefab;
    public void SetSpawnPoint(Transform point) => spawnPoint = point;
}
