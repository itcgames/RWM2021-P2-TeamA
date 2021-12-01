using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{
    public Sprite[] sprites;
    public int leftSprite;
    public int rightSprite;
    public int upSprite;
    public int downSprite;
    private SpriteRenderer _sprite;
    public GameObject player;
    public bool hasShield;
    [HideInInspector]
    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = sprites[leftSprite];
        direction = Vector2.left;
        if(hasShield)
        {
            int pickedDirection = Random.Range(1, 4);
            if(pickedDirection == leftSprite)
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player.GetComponent<TestInventoryPlayer>().IsAttacking())
            {
                return;
            }
            player.GetComponent<TestPlayer>().TakeDamage(1);
            Debug.Log("Player took damage");
        }
    }
}
