﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestItemSpawner : MonoBehaviour
{
    public GameObject item;
    public GameObject player;
    public float timeToWaitBetweenSpawns;
    public Transform[] spawnLocations;
    List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while(true)
        {

            CreateItem();
            yield return new WaitForSeconds(timeToWaitBetweenSpawns);
        }
    }

    

    private void Update()
    {
        foreach(GameObject item in items)
        {
            if(item.GetComponent<TestItem>().spawner == null)
            {
                item.GetComponent<TestItem>().spawner = gameObject;
            }
            
        }
    }

    public GameObject CreateItem()
    {
        Instantiate(item);
        item.GetComponent<TestItem>().spawner = gameObject;
        if(items == null)
        {
            items = new List<GameObject>();
        }
        items.Add(item);
        if (item.tag == "Potion")
        {
            PotionScript script = item.GetComponent<PotionScript>();
            script.IsRedPotion = true;
            script.IsBluePotion = false;
        }
        if (spawnLocations != null && spawnLocations.Length > 0)
        {
            foreach (Transform trans in spawnLocations)
            {
                Vector2 postion = item.transform.position;
                item.transform.position = trans.position;
                Collider2D[] collider2Ds;
                collider2Ds = Physics2D.OverlapCircleAll(trans.position, 0.2f);
                List<Collider2D> colliders = collider2Ds.ToList();
                colliders = colliders.Where(x => x.gameObject.tag != item.gameObject.tag).ToList();
                if (colliders.Count == 0)
                {
                    return item;
                }
                else
                {
                    item.transform.position = postion;
                }
            }
        }
        else
        {
            item.transform.position = transform.position;
        }
       
        
        return item;
    }
}
