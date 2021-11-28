using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public CameraFollowSnap cameraMover;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraMover)
            cameraMover.BeginMovementCallbacks.Add(OnCameraBeginMovement);
    }

    private void OnCameraBeginMovement()
    {
        // Gets all the items and entities in the scene.
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Destroys all retrieved items and enemies.
        foreach (GameObject item in items) Destroy(item);
        foreach (GameObject enemy in enemies) Destroy(enemy);
    }
}
