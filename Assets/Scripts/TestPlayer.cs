using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    public int maxHealth;
    private int _health;
    public Image[] hearts;

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
        }
        else
        {
            _health -= damage;
        }

    }

    public void HealPlayerToFull()
    {
        _health = maxHealth;
    }
}
