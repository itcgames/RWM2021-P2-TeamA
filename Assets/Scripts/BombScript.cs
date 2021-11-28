using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DetonateBomb()
    {
        yield return new WaitForSeconds(timeToDetonate);
        ParticleSystem particles = GetComponentInParent<ParticleSystem>();
        particles.Play();
        SpriteRenderer sprite = GetComponentInParent<SpriteRenderer>();
        sprite.enabled = false;
        yield return new WaitForSeconds(particles.duration);
        Destroy(gameObject);
    }
}
