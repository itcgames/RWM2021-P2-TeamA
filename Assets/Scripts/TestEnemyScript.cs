using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player.GetComponent<TestInventoryPlayer>().IsAttacking())
            {
                Instantiate(destroyParticleEffect);
                destroyParticleEffect.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject);               
            }
            if(hurtPlayerOnCollision)
            {
                player.GetComponent<TestPlayer>().TakeDamage(1);
                Debug.Log("Player took damage");
            }
            
        }
        else if(collision.gameObject.tag == "Sword")
        {
           
            Destroy(collision.gameObject);
            health -= damageAmount;
            if(health <= 0)
            {
                GameObject particleEffect = Instantiate(destroyParticleEffect);
                particleEffect.transform.position = transform.position;
                particleEffect.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject);
            }
                            
        }
    }

    public void PlayParticleEffect()
    {
        GameObject particleEffect = Instantiate(destroyParticleEffect);
        particleEffect.transform.position = transform.position;
        particleEffect.GetComponent<ParticleSystem>().Play();
    }
}
