using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float health = 1.0f;

    public float damageCooldown = 1.0f; // In seconds.
    public int numberOfFlashes = 4;

    private float lastHitTime = 0.0f;

    public void TakeDamage(float damage)
    {
        if (lastHitTime + damageCooldown < Time.time)
        {
            health -= damage;
            lastHitTime = Time.time;

            // If the health is zero, destroys the object, otherwise flashes.
            if (health <= 0.0f)
                Destroy(gameObject);
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
