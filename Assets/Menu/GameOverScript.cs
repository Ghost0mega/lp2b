using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class GameOverScript : MonoBehaviour
{
    [Header("Stuff needed")]
    public TMP_FontAsset[] fonts;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI restartButtonText;
    public TextMeshProUGUI exitButtonText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = 8f;
        transform.position = pos;
        StartCoroutine(DropDown());
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
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
