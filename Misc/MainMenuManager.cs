using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float sceneLoadDelay = 0.13f;

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadSceneWithDelay(int buildIndex)
    {
        StartCoroutine(LoadSceneDelayed(buildIndex));
    }
    private IEnumerator LoadSceneDelayed(int buildIndex)
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
