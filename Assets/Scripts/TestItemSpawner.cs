using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemSpawner : MonoBehaviour
{
    public GameObject item;
    public float timeToWaitBetweenSpawns;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while(true)
        {
            Instantiate(item);
            item.GetComponent<TestItem>().prefab = item;
            yield return new WaitForSeconds(timeToWaitBetweenSpawns);
        }
    }
}
