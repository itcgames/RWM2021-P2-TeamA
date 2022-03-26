using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public int numberToSpawn;
    public int limit = 20;
    public float rate;
    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject obj = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)], transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(rate);
        }
    }
}
