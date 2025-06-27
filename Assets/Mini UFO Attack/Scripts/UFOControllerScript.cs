using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UFOControllerScript : MonoBehaviour
{
    private float scoreTimer = 0f;
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private PlayerScript playerScript;
    private Vector3 playerSpawnPoint;
    [SerializeField] private int defaultBulletLayerCount;
    [SerializeField] private float defaultShootCooldown;
    [SerializeField] private int defaultLives;


    [Header("Enemy spawning settings")]
    [SerializeField] private float minSpawnInterval = 3f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float timeToLowestSpawnInterval = 60f;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float[] spawnProbabilities;
    private List<GameObject> enemies;
    // [SerializeField] private Transform playerTransform; // Reference to the player transform for enemy targeting
    private float spawnTimer;
    private float initialMinSpawnInterval;

    [Header("UI Settings")]
    [SerializeField] private GameObject ufoCanvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedUpgradeText;
    [SerializeField] private TextMeshProUGUI burstUpgradeText;
    [SerializeField] private List<UnityEngine.UI.Image> livesImages;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Scores & sutch")]
    public int score = 0;
    public int speedUpgrade = 0;
    public int burstUpgrade = 0;
    private bool isGameRunning = false;


    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in UFOControllerScript.");
            return;
        }
        playerSpawnPoint = new Vector3(-6f, 0f, 5f);
        ResetGame();
        isGameRunning = true;
    }

    private void Update()
    {
        if (!isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                return;
            }
        }
        else
        {
            // Increment score by 1 every second
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                score++;
                scoreTimer -= (int)scoreTimer;
            }
            Debug.Log("Score: " + score);
            float elapsedTime = Time.timeSinceLevelLoad;
            float lerpFactor = Mathf.Clamp01(elapsedTime / timeToLowestSpawnInterval);
            minSpawnInterval = Mathf.Lerp(initialMinSpawnInterval, 1f, lerpFactor);
            minSpawnInterval = Mathf.Lerp(initialMinSpawnInterval, 1f, lerpFactor);

            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnRandomEnemy();
                spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
            }
            uiUpdateAll();
            if (playerScript != null && playerScript.lives <= 0)
            {
                GameOver();
            }
        }
            
        
    }

    public void ResetGame()
    {
        score = 0;
        speedUpgrade = 0;
        burstUpgrade = 0;

        if (ufoCanvas != null)
        {
            ufoCanvas.SetActive(true);
        }

        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
            playerScript = player.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.bulletLayerCount = defaultBulletLayerCount;
                playerScript.shootCooldown = defaultShootCooldown;
                playerScript.lives = defaultLives;
                playerScript._controller = gameObject; 
            }
            else
            {
                Debug.LogError("PlayerScript component not found on the player prefab.");
            }
        }
        else
        {
            Debug.LogError("Player prefab is not assigned in UFOControllerScript.");
        }
        isGameRunning = true;
        initialMinSpawnInterval = minSpawnInterval;
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        uiUpdateAll();
    }

    public void GameOver()
    {
        isGameRunning = false;
        if (player != null)
        {
            Destroy(player);
            player = null;
            playerScript = null;
        }
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            enemies.Clear();
        }
        if (gameOverPanel != null)
        {
            GameObject panel = Instantiate(gameOverPanel, ufoCanvas.transform);
            GameOverScript gameOverScript = panel.GetComponent<GameOverScript>();
            if (gameOverScript != null)
            {
                gameOverScript.finalScore = score;
                gameOverScript.ufoController = this;
            }
            else
            {
                Debug.LogError("GameOverScript component not found on the game over panel.");
            }
        }
    }
    private void SpawnRandomEnemy()
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
            enemyScript.playerTransform = player.transform;
            enemyScript._controllerScript = this;
        }
        if (enemies == null)
        {
            enemies = new List<GameObject>();
        }
        enemies.Add(enemy);
    }

    public void uiUpdateAll()
    {
        uiUpdateScore(score);
        uiUpdateSpeed(speedUpgrade);
        uiUpdateBurst(burstUpgrade);
        uiUpdateLives(playerScript != null ? playerScript.lives : defaultLives);
    }

    private void uiUpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("D5");
        }
    }

    private void uiUpdateSpeed(int speedUpgrades)
    {
        if (speedUpgradeText != null)
        {
            speedUpgradeText.text = speedUpgrades.ToString();
        }
    }

    private void uiUpdateBurst(int burstUpgrades)
    {
        if (burstUpgradeText != null)
        {
            burstUpgradeText.text = burstUpgrades.ToString();
        }
    }

    public void uiUpdateLives(int lives)
    {
        if (livesImages != null && livesImages.Count > 0)
        {
            for (int i = 0; i < livesImages.Count; i++)
            {
                if (i < lives)
                {
                    livesImages[i].enabled = true;
                }
                else
                {
                    livesImages[i].enabled = false;
                }
            }
        }
    }
}
