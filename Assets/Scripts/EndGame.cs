using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TopdownCharacterController;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndGame : MonoBehaviour
{
    public void MenuReturn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void CloseGame()
    {
        Time.timeScale = 1;
        Application.Quit();
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
