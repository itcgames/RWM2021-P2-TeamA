using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject item;
    public GameObject player;
    public float timeToWaitBetweenSpawns;
    public Transform[] spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {

            CreateItem();
            yield return new WaitForSeconds(timeToWaitBetweenSpawns);
        }
    }

    public GameObject CreateItem()
    {
        Instantiate(item);
        item.GetComponent<TestEnemyScript>().player = player;
        item.transform.localScale = new Vector3(12, 12, 12);
        foreach (Transform trans in spawnLocations)
        {
            Vector2 postion = item.transform.position;
            item.transform.position = trans.position;
            Collider2D[] collider2Ds;
            collider2Ds = Physics2D.OverlapCircleAll(trans.position, 0.2f);
            List<Collider2D> colliders = collider2Ds.ToList();
            colliders = colliders.Where(x => x.gameObject == item.gameObject).ToList();
            if (colliders.Count == 0)
            {
                return item;
            }
            else
            {
                item.transform.position = postion;
            }
        }

        return item;
    }
}
