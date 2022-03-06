using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth;
    [SerializeField]
    private float _health = 1.0f;

    public float damageCooldown = 1.0f; // In seconds.
    public int numberOfFlashes = 4;

    private float lastHitTime = 0.0f;

    [System.Serializable]
    public class GameState
    {
        public int completion_time;
        public int level;
        public string eventName;
        public string deviceUniqueIdentifier;
    }


    private void Start()
    {
        _maxHealth = _health;
        lastHitTime = Time.time - damageCooldown;
    }

    public void HealToFull()
    {
        _health = _maxHealth;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void TakeDamage(float damage, string weaponUsed="")
    {
        if (lastHitTime + damageCooldown < Time.time)
        {
            _health -= damage;
            lastHitTime = Time.time;

            if(gameObject.tag == "Player")
            {
                Player tPlayer = gameObject.GetComponent<Player>();
                tPlayer.TakeDamage(1);
            }

            // If the health is zero, destroys the object, otherwise flashes.
            if (_health <= 0.0f)
            {
                if (gameObject.tag == "Enemy")
                {
                    EnemyScript script = gameObject.GetComponent<EnemyScript>();
                    if (script != null)
                    {
                        script.GenerateItemPossibility();
                        script.PlaceItem();
                        script.OnKillOccurs(weaponUsed);
                        script.PlayParticleEffect();
                    }
                }
                Destroy(gameObject);
                
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
