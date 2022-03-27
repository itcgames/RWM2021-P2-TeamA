using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float secondsDelay;

    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(secondsDelay);
        Destroy(gameObject);
    }
}
