using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 0.5f;

    // Used to avoid projectiles disappearing while still in view.
    private const float EXTRA_BOUNDS_MARGIN = 2.0f;

    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    private CameraFollowSnap.Bounds _bounds;

    public void Fire(Vector2 velocity, CameraFollowSnap.Bounds bounds)
    {
        // Assigns the velocity to the rigidbody or stores it if no rb exists.
        if (_rb) _rb.velocity = velocity;
        else _velocity = velocity;

        _bounds = bounds;

        // Adds the bounds margin.
        _bounds.right += EXTRA_BOUNDS_MARGIN;
        _bounds.left -= EXTRA_BOUNDS_MARGIN;
        _bounds.top += EXTRA_BOUNDS_MARGIN;
        _bounds.bottom -= EXTRA_BOUNDS_MARGIN;
    }

    void Start()
    {
        // Gets the rigidbody, disables the script if null.
        _rb = GetComponent<Rigidbody2D>();
        if (_rb) _rb.velocity = _velocity;
        else enabled = false;
        
    }

    private void Update()
    {
        // Destroys the object if outside the bounds.
        if (transform.position.x > _bounds.right ||
            transform.position.x < _bounds.left ||
            transform.position.y > _bounds.top ||
            transform.position.y < _bounds.bottom)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deals damage to the collider if it has a health component.
            collision.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
