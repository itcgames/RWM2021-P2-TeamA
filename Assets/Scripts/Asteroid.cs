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
    public Vector2 Velocity { get; private set; }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Health.DeathCallbacks.Add(OnDeath);
        PickDirection();
        cam = Camera.main;
    }

    public void PickDirection()
    {
        _velocity.y = (Random.value < 0.5f) ? -1 : 1;
        Movement.PreferHorizontal = true;
        Movement.DiagonalMovement = true;
        Movement.MoveLeft(_persistentMovement);
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
        _velocity.y = (Random.value < 0.5f) ? -1 : 1;
        Movement.PreferHorizontal = true;
        Movement.DiagonalMovement = true;
        Movement.MoveLeft(_persistentMovement);
        if (_velocity.y == -1)
        {
            Movement.MoveUp(_persistentMovement);
        }
        else
        {
            Movement.MoveDown(_persistentMovement);
        }
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

    private void OnBecameInvisible()
    {
        if(cam != null)
        {
            Vector2 viewportPosition = cam.WorldToViewportPoint(transform.position);

            //if it goes off of the right hand screen we just want to destroy it we dont want it to screen wrap as well
            if (viewportPosition.x < 0)
            {
                AsteroidData.numberAsteroidsMissed += 1;
                Destroy(gameObject);
                return;
            }

            //if it goes off of the top or the bottom of the screen then just wrap around to the other side of the screen.
            if (viewportPosition.y > 1 || viewportPosition.y < 0)
            {
                transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
            }
        }
    }
}
