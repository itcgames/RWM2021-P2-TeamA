using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopdownCharacterController;

public class Asteroid : CharacterBehaviour
{
    public GameObject nextAsteroid;
    private Vector2 _velocity = new Vector2(-1, 0);
    private bool _persistentMovement = true;
    public string Type;
    private Camera cam;
    [SerializeField]
    private float timeToDestroy;
    public Vector2 Velocity { get; private set; }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Health.DeathCallbacks.Add(OnDeath);
        PickDirection();
        cam = Camera.main;
        StartCoroutine(DestroyAsteroid());
    }

    public void PickDirection()
    {
        Movement.PreferHorizontal = false;

        Movement.DiagonalMovement = true;

        if(_velocity.y == -1)
        {
            Movement.MoveUp(_persistentMovement);
        }
        else
        {
            Movement.MoveDown(_persistentMovement);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _velocity = dir;
    }

    private void OnDeath(Dictionary<string, string> damageInfo)
    {
        if(nextAsteroid != null)
        {
            AsteroidData.asteroidsDestroyed += 1;
            AsteroidData.asteroidsSpawned += 1;
            Instantiate(nextAsteroid, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DestroyAsteroid()
    {
        yield return new WaitForSeconds(timeToDestroy);
        AsteroidData.numberAsteroidsMissed += 1;
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        if(cam != null)
        {
            Vector2 viewportPosition = cam.WorldToViewportPoint(transform.position);
            if (viewportPosition.y > 1 || viewportPosition.y < 0)
            {
                transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
            }

            if (viewportPosition.x > 1 || viewportPosition.x < 0)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
            }

        }
    }
}
