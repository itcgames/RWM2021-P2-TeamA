using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Header("Properties")]
    public int maxEnemiesPerRoom = 6;
    public int minEnemiesPerRoom = 4;
    public GameObject[] enemyPrefabs;

    [Header("References")]
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

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs != null && enemyPrefabs.Length > 0)
        {
            // Gets the number of enemies to spawn in this area.
            int numberOfEnemies = Mathf.RoundToInt(
                Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom));

            // Gets the bounds of the room.
            Vector3 camPos = cameraMover.transform.position;
            Vector3 halfAreaSize = cameraMover.HalfAreaSize; // Casts from vec2 to vec3.
            Vector3 boundaryOffset = cameraMover.boundaryOffset; // Casts from vec2 to vec3.

            Bounds areaBounds = new Bounds
            {
                min = camPos - halfAreaSize + boundaryOffset,
                max = camPos + halfAreaSize - boundaryOffset
            };

            // Loops for each enemy to spawn.
            for (int i = 0; i < numberOfEnemies; ++i)
            {
                // Loops until a valid spawn if found.
                while (true)
                {
                    // Picks a random location within the camera area.
                    Vector3 position = new Vector3(
                        Random.Range(areaBounds.min.x, areaBounds.max.x),
                        Random.Range(areaBounds.min.y, areaBounds.max.y));

                    // Continue if there's already something in the chosen location.
                    if (Physics2D.OverlapCircle(position, 1.0f))
                        continue;

                    // Spawns a random enemy of a random type.
                    int enemyType = Random.Range(0, enemyPrefabs.Length);
                    Instantiate(enemyPrefabs[enemyType], position, transform.rotation);
                    break; // Breaks from the while now that an enemy is spawned.
                }
            }
        }
    }
}
