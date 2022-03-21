using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnBecameInvisible()
    {
        transform.position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x * 2), transform.position.y, transform.position.z);
    }
}
