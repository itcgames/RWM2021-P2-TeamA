using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    public float health = 1.0f;

    public CameraFollowSnap.Bounds AreaBounds { get; set; }

    protected void Update()
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

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0.0f)
            Destroy(gameObject);
    }
}
