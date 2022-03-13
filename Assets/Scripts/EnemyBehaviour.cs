using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : TopdownCharacterController.CharacterBehaviour
{
    public float maxFireInterval = 10.0f;

    public const float TIME_BETWEEN_ACTIONS = 1.0f;

    // The time at which the last action occurred.
    private float _lastMovementChange;
    private float _lastShotFired;

    public string enemyType;

    // For testing purposes.
    public float GetLastShotFiredTime()
    {
        return _lastShotFired;
    }

    new void Start()
    {
        base.Start();

        Health.DeathCallbacks.Add(OnDeath);

        _lastMovementChange = Time.time - TIME_BETWEEN_ACTIONS;
        _lastShotFired = Time.time;
    }

    void Update()
    {
        if (_lastMovementChange + TIME_BETWEEN_ACTIONS <= Time.time)
        {
            // Reset and pick a new action.
            _lastMovementChange = Time.time;
            Movement.ClearPersistentInput();

            TakeRandomAction();
            ChanceToFireProjectile();
        }
    }

    private void ChanceToFireProjectile()
    {
        // Works out the chance to fire. Get's closer to 100% as time without
        //      firing increases.
        float timeSinceLastShot = Time.time - _lastShotFired;
        int shotChance = (int)Mathf.Max(maxFireInterval - timeSinceLastShot, 0.0f);

        if (Random.Range(0, shotChance) == 0)
        {
            _lastShotFired = Time.time; // Resets the fired time.
            RangedAttack.Fire(Movement.Direction);
        }
    }

    private void TakeRandomAction()
    {
        int action = Random.Range(0, 5);

        // Moves in a direction depending on the action number. 0 == no movement.
        switch (action)
        {
            case 1:
                Movement.MoveLeft(true);
                break;
            case 2:
                Movement.MoveRight(true);
                break;
            case 3:
                Movement.MoveDown(true);
                break;
            case 4:
                Movement.MoveUp(true);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gets the player's health component and damages it if it exists.
            Health health = collision.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(0.5f, "melee", enemyType);
            }
        }
    }

    private void OnDeath(Dictionary<string, string> damageInfo)
    {
        EnemyScript script = gameObject.GetComponent<EnemyScript>();
        if (script != null)
        {
            script.GenerateItemPossibility();
            script.PlaceItem();
            if (damageInfo.ContainsKey("weapon_name"))
                script.OnKillOccurs(damageInfo["weapon_name"]);
            script.PlayParticleEffect();
        }
    }
}
