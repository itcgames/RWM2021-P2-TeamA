using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
	[Header("Properties")]
	public int minEnemiesPerSpawn = 1;
	public int maxEnemiesPerSpawn = 3;
	public float enemySpawnDelay = 5.0f; // The number of seconds between spawn delay.
	public float enemySpawnIncrease = 0.01f; // The decrease in seconds of the spawn delay every second.
	public float spawnMargin = 0.5f;
	public float spawnSpacing = 1.0f;
	public float approxEnemyRadius = 1.5f;

	public GameObject[] enemyPrefabs;

	[Header("References")]
	public Camera _camera;
	public GameObject spawnSmokePrefab;
	public GameObject player;
	public Rigidbody2D playerRigidbody;

	// Private variables.
	private const float _POSITION_ATTEMPTS = 15.0f;

	private float _lastSpawnTime = 0.0f;


	public void ClearAllEntities()
	{
		// Gets all the items, enemies, and projectiles in the scene.
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

		// Destroys all retrieved items, enemies, and projectiles.
		foreach (GameObject item in items) Destroy(item);
		foreach (GameObject enemy in enemies) Destroy(enemy);
		foreach (GameObject projectile in projectiles) Destroy(projectile);
	}

	void Start() => _lastSpawnTime = Time.time;

	void Update()
	{
		if (Time.time > _lastSpawnTime + enemySpawnDelay)
		{
			_lastSpawnTime = Time.time;

			int numberOfEnemies = 
				Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn + 1);

			// Works out the current spawn bounds.
			Bounds spawnBounds = new Bounds{
				min = _camera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f))
					+ new Vector3(spawnMargin, spawnMargin),

				max = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f))
					+ new Vector3(spawnMargin + spawnSpacing, -spawnMargin)
			};

			// Loops for each enemy to spawn.
			for (int i = 0; i < numberOfEnemies; i++)
			{
				// Finds a random valid location.
				Vector3? position = FindValidEnemySpawn(spawnBounds);

				if (position != null)
                {
					// Spawns an enemy of a random type.
					int enemyType = Random.Range(0, enemyPrefabs.Length);
					SpawnEnemy(enemyPrefabs[enemyType], (Vector3)position);
				}					
			}
		}

		enemySpawnDelay -= enemySpawnIncrease * Time.deltaTime;
	}

	//private IEnumerator SpawnEnemies()
	//{
	//	// Checks that the necessary prefabs exist.
	//	if (spawnSmokePrefab != null 
	//		&& enemyPrefabs != null && enemyPrefabs.Length > 0)
	//	{
	//		// Gets the number of enemies to spawn in this area.
	//		int numberOfEnemies = Mathf.RoundToInt(
	//			Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom));

	//		// For storing references for later.
	//		EnemyBehaviour[] enemies = new EnemyBehaviour[numberOfEnemies];
	//		GameObject[] smoke = new GameObject[numberOfEnemies];

	//		// Gets the bounds of the room.
	//		CameraFollowSnap.Bounds areaBounds = cameraMover.GetBounds();

	//		// Loops for each enemy to spawn.
	//		for (int i = 0; i < numberOfEnemies; ++i)
	//		{
	//			// Finds a random location within the camera area.
	//			Vector3 position = FindValidEnemySpawn(areaBounds);

	//			// Spawns an enemy of a random type.
	//			int enemyType = Random.Range(0, enemyPrefabs.Length);
	//			EnemyBehaviour enemy = SpawnEnemy(enemyPrefabs[enemyType],
	//											  position, areaBounds);

	//			// If the enemy was spawned correctly.
	//			if (enemy)
	//			{
	//				// Takes a reference to the enemy.
	//				enemies[i] = enemy;
	//				enemy.enabled = false;

	//				// Spawns a smoke prefab in the enemy location.
	//				smoke[i] = SpawnSmoke(position);
	//			}
	//		}

	//		// Waits for the enemy spawn time and then reveals the enemy.
	//		yield return new WaitForSeconds(enemySpawnSeconds);

	//		foreach (GameObject smokeObj in smoke)
	//			Destroy(smokeObj);

	//		foreach (EnemyBehaviour enemy in enemies)
	//			if (enemy)
	//				enemy.enabled = true;
	//	}

	//	yield return null;
	//}

	public EnemyBehaviour SpawnEnemy(GameObject prefab, Vector3 position)
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

	//private GameObject SpawnSmoke(Vector3 position)
	//{
	//	// Changes the z so the smoke always appears on top.
	//	position.z -= 5.0f;

	//	// Spawns a smoke prefab in the enemy location.
	//	return Instantiate(spawnSmokePrefab, position, transform.rotation);
	//}

	private Vector3? FindValidEnemySpawn(Bounds spawnBounds)
	{
		Vector3? position = null;

		// Loops until a valid spawn if found.
		for (int i = 0; i < _POSITION_ATTEMPTS; i++)
		{
			// Picks a random location within the area bounds.
			position = new Vector3(
				Random.Range(spawnBounds.min.x, spawnBounds.max.x),
				Random.Range(spawnBounds.min.y, spawnBounds.max.y)
			);

			// Breaks if there's nothing in the chosen location.
			if (Physics2D.OverlapCircle((Vector2)position, approxEnemyRadius) == null)
				break;
		}

		return position;
	}
}
