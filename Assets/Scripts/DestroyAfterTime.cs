using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }
    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
