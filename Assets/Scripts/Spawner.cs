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
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
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
        AsteroidData.asteroidsSpawned += 1;

     
        Bounds spawnBounds = new Bounds
        {
            min = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f)),

            max = cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f))
        };
       
        float top = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float bottom = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        Vector2 dir = new Vector2(0, (Random.value < 0.5f) ? -1 : 1);

        Vector3 position = new Vector3(
            Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            (dir.y == 1) ? top : bottom
        );
        GameObject obj = Instantiate(ObjectToSpawn(), position, Quaternion.identity);
        obj.GetComponent<Asteroid>().SetDirection(dir);
    }

    public void GenerateObjectPossibility()
    {
        Probability = (uint)Random.Range(1, 100);
    }
}
