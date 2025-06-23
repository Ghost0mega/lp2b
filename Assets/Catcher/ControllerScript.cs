using UnityEngine;
using TMPro;

public class ControllerScript : MonoBehaviour
{
    public static int score = 0; // Score variable to keep track of the score
    public bool isGameRunning = true; // Variable to control the game state

    public float maxTime = 90.0f;
    private float timeRemaining;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StartGame();
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
    }

    private void StartGame()
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
        timerText.text = "Time's up!";
        scoreText.text = "Final Score: " + score;
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
