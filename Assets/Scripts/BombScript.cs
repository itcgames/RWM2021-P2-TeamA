using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public Vector2 direction;
    public float timeToDetonate;
    public int radius;
    public float damage = 10.0f;
    public string AttackerTag = "Player";
    private Dictionary<string, string> AttackInfo = new Dictionary<string, string>();

    void Start()
    {
        AttackInfo.Add("weapon_name", "bomb");
    }

    public void InitialiseBasics(Vector2 position)
    {
        transform.position = position;
        gameObject.AddComponent<ParticleSystem>();
        gameObject.SetActive(true);
    }

    public IEnumerator DetonateBomb()
    {
        yield return new WaitForSeconds(timeToDetonate);
        ParticleSystem particles = gameObject.GetComponent<ParticleSystem>();
        particles.Play();
        Collider2D[] collider2Ds;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
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
                        enemyBehaviour.Health.TakeDamage(damage, AttackerTag, AttackInfo);
                    }
                    else if (direction == Vector2.down && enemyDirection != Vector2.up)
                    {
                        enemyBehaviour.Health.TakeDamage(damage, AttackerTag, AttackInfo);
                    }
                    if (direction == Vector2.left && enemyDirection != Vector2.right)
                    {
                        enemyBehaviour.Health.TakeDamage(damage, AttackerTag, AttackInfo);
                    }
                    if (direction == Vector2.right && enemyDirection != Vector2.left)
                    {
                        enemyBehaviour.Health.TakeDamage(damage, AttackerTag, AttackInfo);
                    }
                }
                else
                {
                    enemyBehaviour.Health.TakeDamage(damage, AttackerTag, AttackInfo);
                }
            }
        }
        yield return new WaitForSeconds(particles.main.duration);
        Destroy(gameObject);
    }
}
