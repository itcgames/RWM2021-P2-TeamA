using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public delegate void OnEntered();

    public List<OnEntered> OnEnteredCallbacks { get; set; }

    private void Awake()
    {
        OnEnteredCallbacks = new List<OnEntered>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (OnEnteredCallbacks.Count > 0)
            {
                foreach (var callback in OnEnteredCallbacks)
                    callback();
            }
        }
    }
}
