using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    public GameObject spawner = null;
    public string textureName;

    public string Name { get => gameObject.GetComponent<InventoryItem>().Name;  }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            uint amount = 1;
            if (tag == "Bomb")
            {
                amount = 4;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<TestInventoryPlayer>().AddBomb((int)amount);
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<TestInventoryPlayer>()
                .AddObjectToInventory(spawner.GetComponent<TestItemSpawner>().item, textureName, gameObject.GetComponent<InventoryItem>().Name, amount);
            Debug.Log("Item: " + Name + " Amount Added: " + amount);
            Destroy(this.gameObject);
        }
    }    
}
