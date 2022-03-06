using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int _health;
    public Image[] hearts;
    public Sprite fadedHeart;
    public Sprite fullHeart;
    //public bool UseHealth;

    [System.Serializable]
    public class GameState
    {
        public int completion_time;
        public int level;
        public string eventName;
        public string deviceUniqueIdentifier;
    }

    public class GameStart
    {
        public string deviceUniqueIdentifier;
        public DateTime dateTime;
        public int eventId;
    }

    public class GameEnd
    {
        public string deviceUniqueIdentifier;
        public DateTime timeEnded;
        public int eventId;
    }

    private void Start()
    {
        _health = maxHealth;
        GameStart gameStart = new GameStart { dateTime = DateTime.UtcNow, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 0 };
        string jsonData = JsonUtility.ToJson(gameStart);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
    }


    // Start is called before the first frame update
    public void TakeDamage(int damage)
    {
        if (damage < 0) return;
        if(damage >= _health)
        {
            _health = 0;
            foreach(Image heart in hearts)
            {
                heart.sprite = fadedHeart;
            }

            GameState data = new GameState { completion_time = 2000, level = 7, eventName = "Player Death", deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier };
            string jsonData = JsonUtility.ToJson(data);
            StartCoroutine(AnalyticsManager.PostMethod(jsonData));
        }
        else
        {
            _health -= damage;
            hearts[_health].sprite = fadedHeart;
        }
    }

    public void HealPlayerToFull()
    {
        _health = maxHealth;
        Health h = gameObject.GetComponent<Health>();
        if(h)
        {
            h.HealToFull();
        }
        foreach (Image heart in hearts)
        {
            heart.sprite = fullHeart;
        }
    }

    public bool IsAtFullHealth()
    {
        return _health == maxHealth;
    }

    private void OnApplicationQuit()
    {
        GameEnd gameEnd = new GameEnd { timeEnded = DateTime.UtcNow, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier };
        string jsonData = JsonUtility.ToJson(gameEnd);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
    }
}
