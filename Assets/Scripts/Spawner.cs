using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject parent;
    public int numberToSpawn;
    public int limit = 20;
    public float rate;
    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = rate;
    }

    // Update is called once per frame
    void Update()
    {
        if(parent.transform.childCount < limit)
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0f)
            {
                for(int i = 0; i < numberToSpawn; ++i)
                {
                    GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                    GameObject obj = Instantiate(objectToSpawn, parent.transform.position, Quaternion.identity);
                    obj.transform.parent = parent.transform;
                }
                spawnTimer = rate;
            }
        }
    }

    float GetModifier()
    {
        float modifier = Random.Range(0f, 1f);
        if(Random.Range(0, 2) > 0)
        {
            return -modifier;
        }
        return modifier;
    }
}
