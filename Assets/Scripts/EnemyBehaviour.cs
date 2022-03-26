using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : TopdownCharacterController.CharacterBehaviour
{

    public const float TIME_BETWEEN_ACTIONS = 1.0f;

    public float maxFireInterval = 10.0f;
    public float waveAmount = 5.0f;

    // The time at which the last action occurred.
    private float _lastShotFired;

    // A random number to make each enemy move in waves differently.
    private float _randWaveStart;

    public string enemyType;

    // For testing purposes.
    public float GetLastShotFiredTime()
    {
        return _lastShotFired;
    }

    new void Start()
    {
        base.Start();

        // Disables the behaviour if the required components are null.
        if (!Movement || !MeleeAttack || !RangedAttack || !Health)
            enabled = false;

        else
        {
            Health.DeathCallbacks.Add(OnDeath);

            MeleeAttack.AttackInfo.Add("weapon_name", "melee");
            MeleeAttack.AttackInfo.Add("enemy_type", enemyType);

            RangedAttack.AttackInfo.Add("weapon_name", "rock");
            RangedAttack.AttackInfo.Add("enemy_type", enemyType);

            Movement.MoveLeft(true);

            _lastShotFired = Time.time;

            _randWaveStart = Random.Range(0.0f, 100.0f);
        }
    }

    void Update()
    {
        float waveValue = _randWaveStart + (transform.position.x / waveAmount);

        if (Mathf.Sin(waveValue) < 0)
            Movement.MoveUp();
        else
            Movement.MoveDown();

        //if (_lastMovementChange + TIME_BETWEEN_ACTIONS <= Time.time)
        //{
        //    // Reset and pick a new action.
        //    _lastMovementChange = Time.time;
        //    Movement.ClearPersistentInput();

        //    TakeRandomAction();
        //    ChanceToFireProjectile();
        //}
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

    private void OnDeath(Dictionary<string, string> damageInfo)
    {
        EnemyScript script = gameObject.GetComponent<EnemyScript>();
        if (script != null)
        {
            script.GenerateItemPossibility();
            script.PlaceItem();
            if (damageInfo != null && damageInfo.ContainsKey("weapon_name"))
                script.OnKillOccurs(damageInfo["weapon_name"]);
            script.PlayParticleEffect();
        }
    }
}
