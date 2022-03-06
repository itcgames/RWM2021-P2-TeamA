using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Sprite[] sprites;
    public bool usingSprites;
    public int leftSprite;
    public int rightSprite;
    public int upSprite;
    public int downSprite;
    private SpriteRenderer _sprite;
    public GameObject player;
    public bool hasShield;
    [HideInInspector]
    public Vector2 direction;
    public GameObject destroyParticleEffect;
    public bool hurtPlayerOnCollision;
    public int health;
    public int damageAmount;
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
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


        if(usingSprites)
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.sprite = sprites[leftSprite];
            direction = Vector2.left;
            if (hasShield)
            {
                int pickedDirection = Random.Range(1, 4);
                if (pickedDirection == leftSprite)
                {
                    _sprite.sprite = sprites[leftSprite];
                    direction = Vector2.left;
                }
                else if (pickedDirection == rightSprite)
                {
                    _sprite.sprite = sprites[rightSprite];
                    direction = Vector2.right;
                }
                else if (pickedDirection == upSprite)
                {
                    _sprite.sprite = sprites[upSprite];
                    direction = Vector2.up;
                }
                else if (pickedDirection == downSprite)
                {
                    _sprite.sprite = sprites[downSprite];
                    direction = Vector2.down;
                }
            }
            transform.localScale = new Vector2(4.0f, 4.0f);
        }       
    }

    public void OnKillOccurs(string weaponUsed)
    {
        KillOccurs killOccurs = new KillOccurs { weaponUsed = weaponUsed, enemyType = enemyType, deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier, eventId = 2 };
        string jsonData = JsonUtility.ToJson(killOccurs);
        StartCoroutine(AnalyticsManager.PostMethod(jsonData));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            Destroy(collision.gameObject);
            health -= damageAmount;
            Health h = GetComponent<Health>();
            if (h)
            {
                h.TakeDamage(damageAmount, "Ranged Weapon");
            }
        }
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
            
            if(probability > 30)//50 % chance to drop something
            {
                int item = Random.Range(0, items.Length);
                GameObject i = Instantiate(items[item]);
                i.transform.position = gameObject.transform.position;
                i.GetComponent<Item>().prefab = items[item];
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
