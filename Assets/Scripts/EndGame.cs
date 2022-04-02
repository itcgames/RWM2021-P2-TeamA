using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TopdownCharacterController;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public GameObject player;
    public GameObject txt;

    public void Start()
    {
        Health health = player.GetComponent<Health>();
        health.DeathCallbacks.Add(DeathCallback);
    }

    private void DeathCallback(Dictionary<string, string> damageInfo)
    {
        Time.timeScale = 0;
        txt.GetComponent<Text>().text = "You Died!";
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
}
