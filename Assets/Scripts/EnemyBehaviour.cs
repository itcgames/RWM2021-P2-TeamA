using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : TopdownCharacterController.CharacterBehaviour
{

    public const float TIME_BETWEEN_ACTIONS = 1.0f;

    public float fireInterval = 2.0f;
    public float waveAmount = 5.0f;
    public string enemyType;

    // A random number to make each enemy move in waves differently.
    private float _randWaveStart;
    private float _lastShotFired;
    private Rigidbody2D _rigidbody;


    // For testing purposes.
    public float GetLastShotFiredTime()
    {
        return _lastShotFired;
    }

    new void Start()
    {
        base.Start();

        _rigidbody = GetComponent<Rigidbody2D>();

        // Disables the behaviour if the required components are null.
        if (!Movement || !MeleeAttack || !RangedAttack || !Health || !_rigidbody)
            enabled = false;

        else
        {
            Health.DeathCallbacks.Add(OnDeath);

            MeleeAttack.AttackInfo.Add("weapon_name", "enemy collision");
            MeleeAttack.AttackInfo.Add("enemy_type", enemyType);

            RangedAttack.AttackInfo.Add("weapon_name", "enemy laser");
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

        if (Time.time >= _lastShotFired + fireInterval)
        {
            _lastShotFired = Time.time;
            RangedAttack.Fire(_rigidbody.velocity.normalized);
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f,
            Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg);
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
