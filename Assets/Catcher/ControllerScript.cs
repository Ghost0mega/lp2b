using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControllerScript : MonoBehaviour
{
    public static int score = 0; // Score variable to keep track of the score
    public bool isGameRunning = true; // Variable to control the game state

    public float maxTime;
    private float timeRemaining;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Game Over Logic")]
    [SerializeField] private GameObject canvas; 
    [SerializeField] private GameObject gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            timeRemaining -= Time.deltaTime; // Decrease the timer
            timerText.text = "Time: " + FormatTime(timeRemaining); // Update the timer text

            if (timeRemaining <= 0)
            {
                StopGame(); // Stop the game when time runs out
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void ResetGame()
    {
        isGameRunning = true;
        SpawnerScript.isSpawning = true; // Start spawning fruits
        timeRemaining = maxTime; // Reset the timer
        score = 0; // Reset the score
        scoreText.text = "Score: " + score; // Update the score text
        timerText.text = "Time: " + FormatTime(timeRemaining); // Update the timer text
    }

    private void StopGame()
    {
        isGameRunning = false;
        SpawnerScript.isSpawning = false; // Stop spawning fruits
        // timerText.text = "Time's up!";
        // scoreText.text = "Final Score: " + score;
        if (gameOverPanel != null && canvas != null)
        {
            GameObject panel = Instantiate(gameOverPanel, canvas.transform);
            GameOverScript gameOverScript = panel.GetComponent<GameOverScript>();
            if (gameOverScript != null)
            {
                gameOverScript.finalScore = score;
                gameOverScript.catcherController = this;
            }
            else
            {
                Debug.LogError("GameOverScript component not found on the game over panel.");
            }
        }
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score + "cL";
    }
    
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
