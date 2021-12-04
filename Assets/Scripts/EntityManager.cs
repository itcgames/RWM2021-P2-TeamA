using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
	[Header("Properties")]
	public int maxEnemiesPerRoom = 6;
	public int minEnemiesPerRoom = 4;
	public float enemySpawnSeconds = 0.5f;
	public GameObject[] enemyPrefabs;

	[Header("References")]
	public GameObject spawnSmokePrefab;
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

		StartCoroutine(SpawnEnemies());
	}

	private IEnumerator SpawnEnemies()
	{
		// Checks that the necessary prefabs exist.
		if (spawnSmokePrefab != null 
			&& enemyPrefabs != null && enemyPrefabs.Length > 0)
		{
			// Gets the number of enemies to spawn in this area.
			int numberOfEnemies = Mathf.RoundToInt(
				Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom));

			// For storing references for later.
			TopdownCharacterController[] enemies = 
				new TopdownCharacterController[numberOfEnemies];
			GameObject[] smoke = new GameObject[numberOfEnemies];

			// Gets the bounds of the room.
			CameraFollowSnap.Bounds areaBounds = cameraMover.GetBounds();

			// Loops for each enemy to spawn.
			for (int i = 0; i < numberOfEnemies; ++i)
			{
				// Finds a random location within the camera area.
				Vector3 position = FindValidEnemySpawn(areaBounds);

				// Spawns an enemy of a random type.
				int enemyType = Random.Range(0, enemyPrefabs.Length);
				GameObject enemy = Instantiate(
					enemyPrefabs[enemyType], position, transform.rotation);

				// Tries to get the character controller and continues if successful.
				var controller = enemy.GetComponent<TopdownCharacterController>();
				if (controller)
				{
					// Takes a reference to the enemy.
					enemies[i] = controller;
					controller.enabled = false;

					// Spawns a smoke prefab in the enemy location.
					GameObject smokeObj = Instantiate(
						spawnSmokePrefab, position, transform.rotation);
					smoke[i] = smokeObj;
				}
				else // If the enemy has no character controller, delete it.
					Destroy(enemy);
			}

			// Waits for the enemy spawn time and then reveals the enemy.
			yield return new WaitForSeconds(enemySpawnSeconds);

			foreach (GameObject smokeObj in smoke)
				Destroy(smokeObj);

			foreach (TopdownCharacterController enemy in enemies)
				if (enemy)
					enemy.enabled = true;
		}

		yield return null;
	}

	private Vector3 FindValidEnemySpawn(CameraFollowSnap.Bounds areaBounds)
	{
		Vector3 position;

		// Loops until a valid spawn if found.
		while (true)
		{
			// Picks a random location within the area bounds.
			position = new Vector3(
				Random.Range(areaBounds.left, areaBounds.right),
				Random.Range(areaBounds.bottom, areaBounds.top)
			);

			// Breaks if there's nothing in the chosen location.
			if (Physics2D.OverlapCircle(position, 1.5f) == null)
				break;
		}

		return position;
	}
}
