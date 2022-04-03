using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool hasShield;
    public GameObject destroyParticleEffect;
    public bool hurtPlayerOnCollision;
    public GameObject[] items;
    public string enemyType;
    private int probability;

    [System.Serializable]
    public class KillOccurs
    {
        public string weaponUsed;
        public string enemyType;
        public int eventId = 2;
        public string deviceUniqueIdentifier;
    }

    /// <summary>
    /// Used to send data to the server to say how the enemy was killed as well as what type of enemy was killed
    /// </summary>
    /// <param name="weaponUsed"></param>
    public void OnKillOccurs(string weaponUsed)
    {
        KillOccurs killOccurs = new KillOccurs { weaponUsed = weaponUsed, enemyType = enemyType, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 2 };
        string jsonData = JsonUtility.ToJson(killOccurs);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
        GameObject go = GameObject.FindGameObjectWithTag("Score");
        ScoreController please = (ScoreController)go.GetComponent(typeof(ScoreController));
        please.IncreaseScore();
    }

    public void PlayParticleEffect()
    {
        GameObject particleEffect = Instantiate(destroyParticleEffect);
        particleEffect.transform.position = transform.position;
        particleEffect.GetComponent<ParticleSystem>().Play();
    }

    public void PlaceItem()
    {
        if (items == null) return;
        int numberOfItems = items.Length;

        if(numberOfItems > 0)
        {
            if (probability > 30)
            {
                int item = Random.Range(0, items.Length);
                GameObject i = Instantiate(items[item]);
                i.transform.position = gameObject.transform.position;
                i.GetComponent<Item>().prefab = items[item];
                i.SetActive(true);
            }
        }
    }

    public void GenerateItemPossibility()
    {
        probability = Random.Range(1, 100);
    }

    public void SetProbability(int possibility)
    {
        probability = possibility;
    }
}
