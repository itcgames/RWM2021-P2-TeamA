using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public GameObject spawner = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TestInventoryPlayer>().UpdateText(spawner.GetComponent<TestItemSpawner>().item);
            Debug.Log("Collision");
            Destroy(this.gameObject);
        }
    }    
}
