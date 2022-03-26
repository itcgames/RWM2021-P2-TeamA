using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public int numberToSpawn;
    public int limit = 20;
    public float rate;
    public bool useCoRoutine = true;
    public uint Probability { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(rate);
        while (true)
        {
            if (!useCoRoutine)
                continue;
            for (int i = 0; i < numberToSpawn; i++)
            {
                GenerateObjectPossibility();
                CreateObject();
            }
            yield return new WaitForSeconds(rate);
        }
    }

    private GameObject ObjectToSpawn()
    {
        if(Probability < 30)
        {
            return objectsToSpawn[0];
        }
        if(Probability < 70)
        {
            return objectsToSpawn[1];
        }
        return objectsToSpawn[2];
    }

    public void CreateObject()
    {
        Instantiate(ObjectToSpawn(), transform.position, Quaternion.identity);
    }

    public void GenerateObjectPossibility()
    {
        Probability = (uint)Random.Range(1, 100);
    }
}
