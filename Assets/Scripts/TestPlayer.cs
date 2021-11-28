using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float health;
    // Start is called before the first frame update
    public void TakeDamage(float damage)
    {
        if (damage < 0) return;
        if(damage >= health)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
    }


}
