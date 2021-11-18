using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            item.GetComponent<TestItem>().spawner = gameObject;
            item.transform.position = gameObject.transform.position;
            yield return new WaitForSeconds(timeToWaitBetweenSpawns);
        }
    }
}
