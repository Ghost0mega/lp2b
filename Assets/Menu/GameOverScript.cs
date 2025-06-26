using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class GameOverScript : MonoBehaviour
{
    [Header("Stuff needed for visuals")]
    public TMP_FontAsset[] fonts; //0 is caligraphy, 1 is futuristic
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI restartButtonText;
    public TextMeshProUGUI exitButtonText;

    [Header("Stuff needed for logic")]
    public int finalScore = 0;

    public ControllerScript catcherController;

    public PlacerScript breakerController;

    // public UFOcontroller ufoController;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = 8f;
        transform.position = pos;
        AdaptToScene();
        finalScoreText.text = "Final Score: " + finalScore.ToString();
        StartCoroutine(DropDown());
    }

    public void BackToMenu()
    {
        // SceneManager.LoadScene("Menu");
        Debug.Log("Back to Menu");
        Destroy(gameObject);
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Catcher")
        {
            catcherController.ResetGame();
        }
        else if (currentScene.name == "Breaker")
        {
            breakerController.ResetGame();
        }
        // else if (currentScene.name == "UFO")
        // {
        //     ufoController.ResetGame();
        // }
        Destroy(gameObject);
    }

    private void AdaptToScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Catcher")
        {
            gameOverText.font = fonts[0]; // Caligraphy font
            finalScoreText.font = fonts[0];
            restartButtonText.font = fonts[0];
            exitButtonText.font = fonts[0];

            gameOverText.fontSize = 20;
            finalScoreText.fontSize = 15;
            restartButtonText.fontSize = 15;
            exitButtonText.fontSize = 15;
        }
        else
        {
            gameOverText.font = fonts[1]; // Futuristic font
            finalScoreText.font = fonts[1];
            restartButtonText.font = fonts[1];
            exitButtonText.font = fonts[1];

            gameOverText.fontSize = 20;
            finalScoreText.fontSize = 15;
            restartButtonText.fontSize = 15;
            exitButtonText.fontSize = 15;
        }
    }

    private float EaseOutBounce(float t)
    {
        if (t < (1 / 2.75f))
        {
            return 7.5625f * t * t;
        }
        else if (t < (2 / 2.75f))
        {
            t -= (1.5f / 2.75f);
            return 7.5625f * t * t + 0.75f;
        }
        else if (t < (2.5f / 2.75f))
        {
            t -= (2.25f / 2.75f);
            return 7.5625f * t * t + 0.9375f;
        }
        else
        {
            t -= (2.625f / 2.75f);
            return 7.5625f * t * t + 0.984375f;
        }
    }

    private IEnumerator DropDown()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(0, 0, 0);
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutBounce(t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Ensure final position is set
    }
}
