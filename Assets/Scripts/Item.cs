using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject spawner = null;
    public string textureName;
    public GameObject prefab;
    public float damageCooldown = 5.0f; // In seconds.
    public int numberOfFlashes = 20;
    private bool _collected = false;
    public string Name { get
        {
            if (gameObject.GetComponent<InventoryItem>() == null) return "Rupee";
           return  gameObject.GetComponent<InventoryItem>().Name;
        } 
    }

    public bool Collected { get => _collected; }

    private void Start()
    {
        StartCoroutine(DestroyItem());
        StartCoroutine(CooldownFlash());
    }

    public void Collect()
    {
        _collected = true;
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
        if(!_collected)
            Destroy(gameObject);
    }   
}
