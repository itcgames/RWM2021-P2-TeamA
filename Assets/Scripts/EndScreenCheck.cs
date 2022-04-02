using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopdownCharacterController;
using UnityEngine.UI;

public class EndScreenCheck : MonoBehaviour
{
    public GameObject player;
    public GameObject endScreen;
    public Text txt;

    private void Start()
    {
        Health health = player.GetComponent<Health>();
        health.DeathCallbacks.Add(DeathCallback);
    }

    private void DeathCallback(Dictionary<string, string> damageInfo)
    {
        Time.timeScale = 0;
        endScreen.SetActive(true);
        txt.text = "You Died!!";
    }
}
