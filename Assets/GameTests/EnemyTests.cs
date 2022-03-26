using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;
using TopdownCharacterController;

public class EnemyTests
{
	private GameObject _basicEnemyPrefab;

	[SetUp]
	public void Setup()
	{
		_basicEnemyPrefab = Resources.Load<GameObject>("Prefabs/BasicEnemy");
		SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
	}

	//   [UnityTest]
	//public IEnumerator SpawnItemOnDeath()
	//{
	//	GameObject octorokObj = SpawnEnemy(_octorokPrefab, "octo");
	//	EnemyScript script = octorokObj.GetComponent<EnemyScript>();

	//	script.SetProbability(51);
	//	script.PlaceItem();
	//	script.SetProbability(29);
	//	script.PlaceItem();

	//	GameObject playerObj = GameObject.Find("Player");
	//	Assert.IsNotNull(playerObj);
	//	Vector3 position = playerObj.transform.position;
	//	playerObj.transform.position = octorokObj.transform.position;

	//	yield return new WaitForSeconds(0.3f);
	//	InventoryPlayer inventory = playerObj.GetComponent<InventoryPlayer>();

	//	int count = inventory.GetNumberOfItems() + inventory.GetNumberOfEquippables();
	//	Assert.NotZero(count);

	//	playerObj.transform.position = position;
	//}

	[UnityTest]
	public IEnumerator BasicEnemyAlwaysMovesLeft()
	{
		GameObject enemyObj = SpawnEnemy(_basicEnemyPrefab, "Enemy");
		yield return null;

		// Takes the enemy position, waits half a second and checks it's moved.
		Vector3 position = enemyObj.transform.position;
		yield return new WaitForSeconds(0.5f);
		Assert.Less(enemyObj.transform.position.x, position.x);
	}

	[UnityTest]
	public IEnumerator BasicEnemyMovesUpAndDown()
	{
		GameObject enemyObj = SpawnEnemy(_basicEnemyPrefab, "Enemy", new Vector3(0.0f, 500.0f));
		yield return null;

		Rigidbody2D rigidbody = enemyObj.GetComponent<Rigidbody2D>();
		Assert.NotNull(rigidbody);

		float yDir = Mathf.Sign(rigidbody.velocity.y);

		yield return new WaitForSeconds(3.8f);
		Assert.AreNotEqual(yDir, Mathf.Sign(rigidbody.velocity.y));
	}

	[UnityTest]
	public IEnumerator BasicEnemyDiesOnHit()
	{
		GameObject enemyObj = SpawnEnemy(_basicEnemyPrefab, "TestEnemy");
		string name = enemyObj.name;
		yield return null;

		// Gets the enemy's health component
		var healthComponent = enemyObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the enemy, waits, checks the enemy is dead.
		healthComponent.TakeDamage(1.0f);
		yield return null;
		Assert.IsNull(GameObject.Find(name));
	}

	[UnityTest]
	public IEnumerator BasicEnemyDealsThornsDamage()
	{
		Health playerHealth = GetPlayersHealthComponent();
		float healthValue = playerHealth.HP;

		// Spawns an enemy and moves it to the player.
		GameObject enemy = SpawnEnemy(_basicEnemyPrefab, "Enemy");
		enemy.transform.position = playerHealth.transform.position;

		// Waits for the collision method to be called.
		yield return new WaitForSeconds(0.1f);

		// Cancels the flash animation to speed up the test.
		playerHealth.StopAllCoroutines();

		// Checks the health has decreased.
		Assert.Less(playerHealth.HP, healthValue);
	}

	private Health GetPlayersHealthComponent()
	{
		// Gets the player and player's health.
		GameObject playerObj = GameObject.Find("Player");
		Assert.IsNotNull(playerObj);

		Health playerHealth = playerObj.GetComponent<Health>();
		Assert.IsNotNull(playerHealth);

		return playerHealth;
	}

	//[UnityTest]
	//public IEnumerator OctorokFiresProjectile()
	//{
	//	GameObject octorokObj = SpawnEnemy(_basicEnemyPrefab, "Octorok");
	//	EnemyBehaviour octorok = octorokObj.GetComponent<EnemyBehaviour>();

	//	// Waits for the octorok to be initialised.
	//	yield return new WaitForSeconds(0.1f);

	//	float timeWaited = 0.0f;
	//	float lastFireTime = octorok.GetLastShotFiredTime();
	//	bool fired = false;

	//	while (timeWaited < octorok.maxFireInterval)
 //       {
	//		// If the last shot fired happened after the captured time.
	//		if (octorok.GetLastShotFiredTime() > lastFireTime)
 //           {
	//			fired = true;
	//			break;
	//		}

	//		timeWaited += 1.0f;
	//		yield return new WaitForSeconds(1.0f);
	//	}

	//	Assert.IsTrue(fired);
	//}

	private GameObject SpawnEnemy(GameObject prefab, string name, Vector3? position = null)
	{
		GameObject entityManagerObj = GameObject.Find("EntityManager");
		Assert.NotNull(entityManagerObj);

		EntityManager entityManager =
			entityManagerObj.GetComponent<EntityManager>();
		Assert.NotNull(entityManager);

		Vector3 pos = Vector3.zero;

		if (position != null) pos = (Vector3)position;

		EnemyBehaviour enemy =
			entityManager.SpawnEnemy(prefab, pos);
		Assert.NotNull(enemy);

		enemy.name = name;

		return enemy.gameObject;
	}
}

