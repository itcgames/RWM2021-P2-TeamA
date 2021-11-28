using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public CameraFollowSnap cameraMover;
    public TopdownCharacterController playerController;
    public Rigidbody2D playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraMover)
        {
            cameraMover.BeginMovementCallbacks.Add(OnCameraBeginMovement);
            cameraMover.EndMovementCallbacks.Add(OnCameraMovementFinished);
        }
    }

    private void OnCameraBeginMovement()
    {
        // Gets all the items and entities in the scene.
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Destroys all retrieved items and enemies.
        foreach (GameObject item in items) Destroy(item);
        foreach (GameObject enemy in enemies) Destroy(enemy);

        // Disables player controller functionality.
        if (playerController)
            playerController.enabled = false;

        // Clears any player velocity.
        if (playerRigidbody)
            playerRigidbody.velocity = Vector2.zero;
    }

    private void OnCameraMovementFinished()
    {
        // Enables player controller functionality.
        if (playerController)
            playerController.enabled = true;
    }
}
