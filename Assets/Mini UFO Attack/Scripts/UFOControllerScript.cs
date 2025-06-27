using UnityEngine;

public class UFOControllerScript : MonoBehaviour
{
    [Header("Enemy spawning settings")]
    [SerializeField] private float minSpawnInterval = 3f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float timeToLowestSpawnInterval = 60f;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float[] spawnProbabilities;
    [SerializeField] private Transform playerTransform; // Reference to the player transform for enemy targeting
    private float spawnTimer;
    private float initialMinSpawnInterval;
    private void Start()
    {
        initialMinSpawnInterval = minSpawnInterval;
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
    
    private void Update(){

        float elapsedTime = Time.timeSinceLevelLoad;
        float lerpFactor = Mathf.Clamp01(elapsedTime / timeToLowestSpawnInterval);
        minSpawnInterval = Mathf.Lerp(initialMinSpawnInterval, 1f, lerpFactor);

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab.Length == 0) return;

        // Safety: If probabilities array is not set up, default to uniform
        if (spawnProbabilities == null || spawnProbabilities.Length != enemyPrefab.Length)
        {
            Debug.LogWarning("Spawn probabilities not set or length mismatch, using uniform probabilities.");
            int randomIndex = Random.Range(0, enemyPrefab.Length);
            SpawnEnemy(enemyPrefab[randomIndex]);
            return;
        }

        float rand = Random.value; // 0.0 to 1.0
        float cumulative = 0f;
        for (int i = 0; i < spawnProbabilities.Length; i++)
        {
            cumulative += spawnProbabilities[i];
            if (rand <= cumulative)
            {
                SpawnEnemy(enemyPrefab[i]);
                return;
            }
        }
        // Fallback (should not happen if probabilities sum to 1)
        SpawnEnemy(enemyPrefab[enemyPrefab.Length - 1]);
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null) return;

        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.playerTransform = playerTransform; // Assign player transform for targeting
        }
    }


}
