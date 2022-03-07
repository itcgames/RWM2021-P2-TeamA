using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private bool offScreen = false;
    private Vector2 returnToScreen;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (offScreen)
        {
            transform.position.Set(transform.position.x + returnToScreen.normalized.x, transform.position.y + returnToScreen.normalized.y, transform.position.z);           
        }        
    }

    private void OnBecameInvisible()
    {
        offScreen = true;
        returnToScreen = new Vector2(transform.position.x - Camera.main.transform.position.x, transform.position.y - Camera.main.transform.position.y);
    }
}
