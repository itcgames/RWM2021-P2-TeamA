using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public int mainMenuBuildIndex = 0;
    public int gameBuildIndex = 1;
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameBuildIndex);
    }

    public void EndGame()
    {
        Application.Quit();
        //if (UnityEditor.EditorApplication.isPlaying)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
    }
}
