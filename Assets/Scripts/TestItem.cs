using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    public GameObject spawner = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SpriteRenderer sprite = spawner.GetComponent<TestItemSpawner>().item.GetComponent<SpriteRenderer>();
            GameObject.FindGameObjectWithTag("Player").GetComponent<TestInventoryPlayer>()
                .AddObjectToInventory(spawner.GetComponent<TestItemSpawner>().item, "Bomb", gameObject.GetComponent<InventoryItem>().Name);
            Debug.Log("Collision");
            Destroy(this.gameObject);
        }
    }    
}
