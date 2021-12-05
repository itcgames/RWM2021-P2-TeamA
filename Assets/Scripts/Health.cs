using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _health = 1.0f;

    public float damageCooldown = 1.0f; // In seconds.
    public int numberOfFlashes = 4;

    private float lastHitTime = 0.0f;

    private void Start()
    {
        lastHitTime = Time.time - damageCooldown;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void TakeDamage(float damage)
    {
        if (lastHitTime + damageCooldown < Time.time)
        {
            _health -= damage;
            lastHitTime = Time.time;

            if(gameObject.tag == "Player")
            {
                TestPlayer tPlayer = gameObject.GetComponent<TestPlayer>();
                tPlayer.TakeDamage(1);
            }

            // If the health is zero, destroys the object, otherwise flashes.
            if (_health <= 0.0f)
            {
                Destroy(gameObject);
                if(gameObject.tag == "Enemy")
                {
                    TestEnemyScript script = gameObject.GetComponent<TestEnemyScript>();
                    if(script != null)
                    {
                        script.PlaceItem();
                    }
                }
            }                
            else
                StartCoroutine(CooldownFlash());
        }
    }

    private IEnumerator CooldownFlash()
    {
        var renderer = GetComponent<Renderer>();

        // If the renderer is not null, continue with the flash functionality.
        if (renderer && renderer.enabled)
        {
            // Works out the variables ahead of time.
            int loops = numberOfFlashes * 2;
            float waitTime = damageCooldown / loops;

            // Loops and flashes.
            for (int i = 0; i < loops; i++)
            {
                renderer.enabled = !renderer.enabled;
                yield return new WaitForSeconds(waitTime);
            }
        }

        yield return null;
    }
}
