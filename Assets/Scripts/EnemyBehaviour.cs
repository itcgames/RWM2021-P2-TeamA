using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    public CameraFollowSnap.Bounds AreaBounds { get; set; }

    public GameObject projectilePrefab;
    public float maxFireInterval = 10.0f;
    public float projectileSpeed = 5.0f;

    public const float TIME_BETWEEN_ACTIONS = 1.0f;

    // The time at which the last action occurred.
    private float _lastMovementChange;
    private float _lastShotFired;

    private Vector2 _direction = Vector2.right;
    private Animator _animator;
    private SpriteRenderer _sprite;

    bool _canAnimate = false;

    // For testing purposes.
    public float GetLastShotFiredTime()
    {
        return _lastShotFired;
    }

    new void Start()
    {
        base.Start();
        _lastMovementChange = Time.time - TIME_BETWEEN_ACTIONS;
        _lastShotFired = Time.time;

        _animator = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        if (_animator && _sprite) _canAnimate = true;
    }

    void Update()
    {
        if (_lastMovementChange + TIME_BETWEEN_ACTIONS <= Time.time)
        {
            // Reset and pick a new action.
            _lastMovementChange = Time.time;
            Controller.ClearPersistentInput();

            TakeRandomAction();
            ChanceToFireProjectile();
        }

        // Keeps to the area bounds.
        StayWithinBoundary();
    }

    private void ChanceToFireProjectile()
    {
        // Breaks from the function immediately if no projectile is set.
        if (!projectilePrefab) return;

        // Works out the chance to fire. Get's closer to 100% as time without
        //      firing increases.
        float timeSinceLastShot = Time.time - _lastShotFired;
        int shotChance = (int)Mathf.Max(maxFireInterval - timeSinceLastShot, 0.0f);

        if (Random.Range(0, shotChance) == 0)
        {
            _lastShotFired = Time.time; // Resets the fired time.

            // Instantiates the projectile and fires it in the last movement direction.
            GameObject proj = Instantiate(projectilePrefab,
                transform.position, Quaternion.identity);

            proj.GetComponent<Projectile>()?.Fire(_direction * projectileSpeed, AreaBounds);
        }
    }

    private void TakeRandomAction()
    {
        int action = Random.Range(0, 5);

        // Moves in a direction depending on the action number. 0 == no movement.
        switch (action)
        {
            case 1:
                Controller.MoveLeft(true);
                _direction = Vector2.left;

                break;
            case 2:
                Controller.MoveRight(true);
                _direction = Vector2.right;
                break;
            case 3:
                Controller.MoveDown(true);
                _direction = Vector2.down;
                break;
            case 4:
                Controller.MoveUp(true);
                _direction = Vector2.up;
                break;
        }

        if (_canAnimate)
        {
            _animator.SetBool("IsVertical", _direction.y != 0.0f);
            _sprite.flipY = _direction.y > 0.0f;
            _sprite.flipX = _direction.x > 0.0f;
        }
    }

    private void StayWithinBoundary()
    {
        Vector3 newPosition = transform.position;

        // Keep to the horizontal bounds.
        if (transform.position.x > AreaBounds.right)
            newPosition.x = AreaBounds.right;
        else if (transform.position.x < AreaBounds.left)
            newPosition.x = AreaBounds.left;

        // Keep to the vertical bounds.
        if (transform.position.y > AreaBounds.top)
            newPosition.y = AreaBounds.top;
        else if (transform.position.y < AreaBounds.bottom)
            newPosition.y = AreaBounds.bottom;

        transform.position = new Vector3(newPosition.x, newPosition.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gets the player's health component and damages it if it exists.
            Health health = collision.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(0.5f);
            }
        }
    }
}
