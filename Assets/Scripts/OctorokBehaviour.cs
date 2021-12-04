using UnityEngine;

public class OctorokBehaviour : EnemyBehaviour
{
    public GameObject projectilePrefab;
    public float maxFireInterval = 10.0f;
    public float projectileSpeed = 5.0f;

    private const float TIME_BETWEEN_ACTIONS = 1.0f;
    private float _lastMovementChange;
    private float _lastShotFired;

    new void Start()
    {
        base.Start();
        _lastMovementChange = Time.time - TIME_BETWEEN_ACTIONS;
        _lastShotFired = Time.time;
    }

    new void Update()
    {
        if (_lastMovementChange + TIME_BETWEEN_ACTIONS <= Time.time)
        {
            // Random chance to fire a projectile.
            ChanceToFireProjectile();

            // Reset and pick a new action.
            _lastMovementChange = Time.time;
            Controller.ClearPersistentInput();
            int action = Random.Range(0, 5);

            // Moves in a direction depending on the action number. 0 == no movement.
            switch (action)
            {
                case 1: Controller.MoveLeft(true); break;
                case 2: Controller.MoveRight(true); break;
                case 3: Controller.MoveDown(true); break;
                case 4: Controller.MoveUp(true); break;
            }
        }

        // Keeps to the area bounds.
        base.Update();
    }

    private void ChanceToFireProjectile()
    {
        float timeSinceLastShot = Time.time - _lastShotFired;
        int shotChance = (int)Mathf.Max(maxFireInterval - timeSinceLastShot, 0.0f);

        if (Random.Range(0, shotChance) == 0)
        {
            _lastShotFired = Time.time; // Resets the fired time.

            Vector2 direction = new Vector2(Random.Range(-1, 2),
                                            Random.Range(-1, 2));

            // Handles cases of two 1s or two 0s.
            if (direction.x != 0.0f && direction.y != 0.0f)
                direction.x = 0.0f;
            else if (direction.x == 0.0f && direction.y == 0.0f)
                direction.x = 1.0f;

            GameObject proj = Instantiate(projectilePrefab,
                transform.position, Quaternion.identity);

            proj.GetComponent<Projectile>()?.Fire(direction * projectileSpeed, AreaBounds);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gets the player's health component and damages it if it exists.
            Health health = collision.GetComponent<Health>();
            if (health)
                health.TakeDamage(0.5f);
        }
    }
}
