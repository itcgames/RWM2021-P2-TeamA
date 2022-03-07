using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Vector2 direction;
    public float timeToDetonate;
    public int radius;
    void Start()
    {
        timeToDetonate = 1.5f;
        StartCoroutine(DetonateBomb());
    }

    private IEnumerator DetonateBomb()
    {
        yield return new WaitForSeconds(timeToDetonate);
        ParticleSystem particles = GetComponentInParent<ParticleSystem>();
        particles.Play();
        SpriteRenderer sprite = GetComponentInParent<SpriteRenderer>();
        sprite.enabled = false;
        Collider2D[] collider2Ds;
        
        collider2Ds = Physics2D.OverlapCircleAll(transform.position, 5.0f);
        List<Collider2D> colliders = collider2Ds.ToList();
        colliders = colliders.Where(x => x.gameObject.tag != "Player")
                            .Where(x => x.gameObject.tag != "Bomb")
                            .Where(x => x.gameObject.tag != "Item").ToList();
        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject.GetComponent<EnemyScript>())
            {
                EnemyScript script = collider.gameObject.GetComponent<EnemyScript>();
                EnemyBehaviour enemyBehaviour = collider.GetComponent<EnemyBehaviour>();
                if(script.hasShield)
                {
                    Vector2 enemyDirection = enemyBehaviour.Movement.Direction;
                    if(direction == Vector2.up && enemyDirection != Vector2.down)
                    {
                        Destroy(collider.gameObject);
                    }
                    else if (direction == Vector2.down && enemyDirection != Vector2.up)
                    {
                        Destroy(collider.gameObject);
                    }
                    if (direction == Vector2.left && enemyDirection != Vector2.right)
                    {
                        Destroy(collider.gameObject);
                    }
                    if (direction == Vector2.right && enemyDirection != Vector2.left)
                    {
                        Destroy(collider.gameObject);
                    }
                }
                else
                {
                    Destroy(collider.gameObject);
                    Debug.Log("Enemy withing blast radius of bomb");
                }
            }
        }
        yield return new WaitForSeconds(particles.duration);
        Destroy(gameObject);
    }
}
