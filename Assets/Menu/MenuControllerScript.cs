using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MenuControllerScript : MonoBehaviour
{
    private IEnumerator StartSceneAfterDelay(string sceneName )
    {
        yield return new WaitForSeconds(1.1f); //Allow time for the animation
        SceneManager.LoadScene(sceneName);
        
    }   

    public void StartCatcher()
    {
        StartCoroutine(StartSceneAfterDelay("Catcher"));
    }


    public void StartBreaker()
    {
        StartCoroutine(StartSceneAfterDelay("Breaker"));
    }

    public void StartUFO()
    {
        StartCoroutine(StartSceneAfterDelay("UFO"));
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
