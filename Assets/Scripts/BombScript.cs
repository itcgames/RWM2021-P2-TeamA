using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Vector2 direction;
    public float timeToDetonate;
    public int radius;
    void Start()
    {
        timeToDetonate = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
