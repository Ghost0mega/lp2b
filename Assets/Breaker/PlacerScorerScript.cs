using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlacerScript : MonoBehaviour
{
    [Header("Placer Settings")]
    [SerializeField] private GameObject BrickPrefab;
    private static float maxX = 5;
    private static float minX = -5f;
    private static float maxY = 3.5f;
    private static float minY = -2f;

    public static float brickSeizeX;
    public static float brickSeizeY;

    private int rows = 10;
    private int columns = 10;

    [Header("Scorer Settings")]
    public int score = 0;
    public int lives = 3;

    private bool playSound = false;

    public List<GameObject> bricks = new List<GameObject>();
    public BallScript ballScript;

    [SerializeField] private TextMeshProUGUI scoreText;
    public List<UnityEngine.UI.Image> livesImages;

    [Header("Game Over Logic")]
    [SerializeField] private GameObject canvas; 
    [SerializeField] private GameObject gameOverPanel;
    private bool gameOverPanelActive = false;

    void Start()
    {
        brickSeizeX = BrickPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        brickSeizeY = BrickPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        PlaceBricks();
        UpdateScoreText();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        if (lives > 0)
        {
            // if (Input.GetMouseButtonDown(0)) // Left mouse button
            // {
            //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //     mousePos.z = 2f;

            //     Instantiate(BrickPrefab, mousePos, Quaternion.identity);
            // }

            if (bricks.Count == 0)
            {
                PlaceBricks(); // Refill bricks if all are destroyed
                AudioManager_BrickBreaker.Instance.PlaySound(AudioType_BrickBreaker.win, AudioSourceType_BrickBreaker.game);
                // ballScript.StartCoroutine(ballScript.SpawnBall(true)); // Respawn the ball

            }

            foreach (GameObject brick in bricks)
            {
                if (brick == null)
                {
                    score += 10; // Increment score for each brick destroyed
                    bricks.Remove(brick);
                    break;
                }
            }
            if (score < 0)
            {
                score = 0; // Prevent negative score
            }
            // score < 0 ? score = 0 : score; why no work :(
            UpdateScoreText();
            UpdateLivesImages();
        }
        else
        {
            // Game over logic
            if (playSound == false)
            {
                AudioManager_BrickBreaker.Instance.PlaySound(AudioType_BrickBreaker.gameover, AudioSourceType_BrickBreaker.game);
                playSound = true;
            }
            if (score < 0)
            {
                score = 0; // Prevent negative score
            }
            // scoreText.text = "Game Over! Final Score: " + score;
            UpdateLivesImages();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame(); // Reset the game when space is pressed
            }

            if (!gameOverPanelActive)
            {
                gameOverPanelActive = true;
                GameObject panel = Instantiate(gameOverPanel, canvas.transform);
                GameOverScript gameOverScript = panel.GetComponent<GameOverScript>();
                if (gameOverScript != null)
                {
                    gameOverScript.finalScore = score;
                    gameOverScript.breakerController = this;
                }
                else
                {
                    Debug.LogError("GameOverScript component not found on the panel.");
                }
            }
        }
    }

    private void PlaceBricks()
    {
        float startX = -columns * brickSeizeX / 2f;
        float startY = maxY;

        for (int row = 0; row < 2 * rows; row++)
        {
            for (int col = 0; col < 2 * columns; col++)
            {
                Vector3 pos = new Vector3(
                    startX + col * brickSeizeX,
                    startY - row * brickSeizeY,
                    2f
                );

                // Only instantiate if inside the field bounds
                if (pos.x > minX && pos.x < maxX && pos.y > minY && pos.y < maxY && Random.Range(0f, 1f) < 0.5f)
                {
                    GameObject newBrick = Instantiate(BrickPrefab, pos, Quaternion.identity);
                    bricks.Add(newBrick);
                }
            }
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateLivesImages()
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

    public void ResetGame()
    {
        lives = 3;
        score = 0;
        UpdateScoreText();
        UpdateLivesImages();
        foreach (GameObject brick in bricks)
        {
            Destroy(brick); // Destroy all existing bricks
        }
        bricks.Clear();
        PlaceBricks(); // Refill bricks
        ballScript.StartCoroutine(ballScript.SpawnBall(true)); // Respawn the ball
        AudioManager_BrickBreaker.Instance.PlaySound(AudioType_BrickBreaker.start, AudioSourceType_BrickBreaker.game);
    }
}
