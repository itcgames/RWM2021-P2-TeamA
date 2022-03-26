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
	public float spawnMargin = 1f;
	public float spawnSpacing = 1.0f;
	public float approxEnemyRadius = 1.5f;
	public float spawnDistance = 5.0f; // The distance over which enemies spawn, none spawn after this distance.

	public GameObject[] enemyPrefabs;

	[Header("References")]
	public Camera _camera;
	public GameObject spawnSmokePrefab;
	public GameObject player;
	public Rigidbody2D playerRigidbody;

	// Private variables.
	private const float _POSITION_ATTEMPTS = 15.0f;

	private float _lastSpawnTime = 0.0f;
	private float _startDistance = 0.0f;


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

	void Start()
	{
		_lastSpawnTime = Time.time;
		_startDistance = _camera.transform.position.x;
	}

	void Update()
	{
		// Calculates and checks the distance travelled is less than the max.
		float distanceTravelled = _camera.transform.position.x - _startDistance;

		if (distanceTravelled < spawnDistance)
		{
			if (Time.time > _lastSpawnTime + enemySpawnDelay)
			{
				_lastSpawnTime = Time.time;

				int numberOfEnemies =
					Random.Range(minEnemiesPerSpawn, maxEnemiesPerSpawn + 1);

				// Works out the current spawn bounds.
				Bounds spawnBounds = new Bounds
				{
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

			// Decreases the spawn delay.
			enemySpawnDelay -= enemySpawnIncrease * Time.deltaTime;
		}
	}

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
