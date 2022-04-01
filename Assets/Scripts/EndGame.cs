using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TopdownCharacterController;

public class EndGame : MonoBehaviour
{

    public GameObject player;
    public Text

    public void Start()
    {
        Health health = player.GetComponent<Health>();
        health.DeathCallbacks.Add(DeathCallback);
    }

    private void DeathCallback()
    {
        txt.text = 
        gameObject.SetActive(true);
    }

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

    private void Death()
    {
        if (player)
    }
}
