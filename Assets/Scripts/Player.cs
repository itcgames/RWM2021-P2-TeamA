﻿using System.Collections;
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



    private void Start()
    {
        _health = maxHealth;
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
}