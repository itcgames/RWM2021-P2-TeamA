using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopdownCharacterController;

public class Asteroid : CharacterBehaviour
{
    public GameObject nextAsteroid;
    private Vector2 _velocity = new Vector2(-1, 0);
    private bool _persistentMovement = true;

    public Vector2 Velocity { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Health.DeathCallbacks.Add(OnDeath);
        PickDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void OnDeath(Dictionary<string, string> damageInfo)
    {
        if(nextAsteroid != null)
        {
            Transform parentTransform = transform.parent;
            GameObject obj = Instantiate(nextAsteroid, parentTransform.position, Quaternion.identity);
            obj.transform.parent = parentTransform;
        }
    }
}
