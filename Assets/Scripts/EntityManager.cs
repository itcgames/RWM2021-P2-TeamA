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
	public GameObject player;
	public Rigidbody2D playerRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		if (cameraMover)
		{
			cameraMover.BeginMovementCallbacks.Add(OnAreaChanged);
			cameraMover.EndMovementCallbacks.Add(OnCameraMovementFinished);
		}
	}

	public void OnAreaChanged()
	{
		// Gets all the items, enemies, and projectiles in the scene.
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

		// Destroys all retrieved items, enemies, and projectiles.
		foreach (GameObject item in items) Destroy(item);
		foreach (GameObject enemy in enemies) Destroy(enemy);
		foreach (GameObject projectile in projectiles) Destroy(projectile);

		// Disables player controller functionality.
		if (player)
			player.SetActive(false);

		// Clears any player velocity.
		if (playerRigidbody)
			playerRigidbody.velocity = Vector2.zero;
	}

	public void UnpausePlayer()
    {
		// Enables player controller functionality.
		if (player)
			player.SetActive(true);
	}

	private void OnCameraMovementFinished()
	{
		UnpausePlayer();
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
			EnemyBehaviour[] enemies = new EnemyBehaviour[numberOfEnemies];
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
				EnemyBehaviour enemy = SpawnEnemy(enemyPrefabs[enemyType],
												  position, areaBounds);

				// If the enemy was spawned correctly.
				if (enemy)
				{
					// Takes a reference to the enemy.
					enemies[i] = enemy;
					enemy.enabled = false;

					// Spawns a smoke prefab in the enemy location.
					smoke[i] = SpawnSmoke(position);
				}
			}

			// Waits for the enemy spawn time and then reveals the enemy.
			yield return new WaitForSeconds(enemySpawnSeconds);

			foreach (GameObject smokeObj in smoke)
				Destroy(smokeObj);

			foreach (EnemyBehaviour enemy in enemies)
				if (enemy)
					enemy.enabled = true;
		}

		yield return null;
	}

	public EnemyBehaviour SpawnEnemy(GameObject prefab, Vector3 position, 
									 CameraFollowSnap.Bounds? bounds = null)
    {
        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
        var behaviour = enemy.GetComponent<EnemyBehaviour>();

		// If no enemy behaviour, destroys the game object and returns null.
        if (!behaviour)
        {
			Destroy(enemy);
            return null;
        }

        return behaviour;
    }

    private GameObject SpawnSmoke(Vector3 position)
    {
		// Changes the z so the smoke always appears on top.
		position.z -= 5.0f;

		// Spawns a smoke prefab in the enemy location.
		return Instantiate(spawnSmokePrefab, position, transform.rotation);
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
