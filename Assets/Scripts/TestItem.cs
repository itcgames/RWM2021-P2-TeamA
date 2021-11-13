using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public GameObject prefab;
    public void Destroy()
    {
        Destroy(this.gameObject);
        prefab = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TestInventoryPlayer>().UpdateText(prefab);
            Debug.Log("Collision");
            Destroy(this.gameObject);
        }
    }    
}
