using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    public GameObject spawner = null;
    public string textureName;
    public GameObject prefab;
    public float damageCooldown = 5.0f; // In seconds.
    public int numberOfFlashes = 20;
    public string Name { get
        {
            if (gameObject.GetComponent<InventoryItem>() == null) return "Rupee";
           return  gameObject.GetComponent<InventoryItem>().Name;
        } 
    }

    private void Start()
    {
        StartCoroutine(DestroyItem());
        StartCoroutine(CooldownFlash());
    }

    private IEnumerator CooldownFlash()
    {
        var renderer = GetComponent<Renderer>();

        // If the renderer is not null, continue with the flash functionality.
        if (renderer && renderer.enabled)
        {
            // Works out the variables ahead of time.
            int loops = numberOfFlashes * 2;
            float waitTime = damageCooldown / loops;

            // Loops and flashes.
            for (int i = 0; i < loops; i++)
            {
                renderer.enabled = !renderer.enabled;
                yield return new WaitForSeconds(waitTime);
            }
        }

        yield return null;
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
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
