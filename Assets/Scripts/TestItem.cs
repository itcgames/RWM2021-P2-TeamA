using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    public GameObject spawner = null;
    public string textureName;
    public GameObject prefab;

    public string Name { get
        {
            if (gameObject.GetComponent<InventoryItem>() == null) return "Rupee";
           return  gameObject.GetComponent<InventoryItem>().Name;
        } 
    }
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
            if(tag == "Potion")
            {
                amount = 2;
            }
            if(spawner != null)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<TestInventoryPlayer>()
                .AddObjectToInventory(spawner.GetComponent<TestItemSpawner>().item, textureName, gameObject.GetComponent<InventoryItem>().Name, amount);
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                TestInventoryPlayer testInventory = player.GetComponent<TestInventoryPlayer>();
                if(tag != "Rupee")
                {
                    testInventory.AddObjectToInventory(prefab, textureName, gameObject.GetComponent<InventoryItem>().Name, amount);
                }
                else
                {
                    player.GetComponent<TestInventoryPlayer>().AddRupee(1);
                }
            }
            Debug.Log("Item: " + Name + " Amount Added: " + amount);
            Destroy(this.gameObject);
        }
    }    
}
