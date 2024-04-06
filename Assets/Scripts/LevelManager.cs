using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        LoadScene("MainMenu");
    }

    public void LoadAboutScene()
    {
        LoadScene("About");
    }

    public void LoadGameOverScene()
    {
        LoadScene("GameOver");
    }

    public void LoadVictoryScene()
    {
        LoadScene("Victory");
    }

    public void LoadGameScene()
    {
        LoadScene("Game");
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private AsyncOperation LoadScene(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }    
}
