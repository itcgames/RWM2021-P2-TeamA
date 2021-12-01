using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnScreenExit : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
