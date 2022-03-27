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
	public IEnumerator BasicEnemyDiesOnThreeHits()
	{
		GameObject enemyObj = SpawnEnemy(_basicEnemyPrefab, "TestEnemy");
		string name = enemyObj.name;
		yield return null;

		// Gets the enemy's health component
		var healthComponent = enemyObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the enemy and waits 3 times and checks the enemy is dead.
		healthComponent.TakeDamage(1.0f);
		yield return new WaitForSeconds(healthComponent.DamageGracePeriod);

		healthComponent.TakeDamage(1.0f);
		yield return new WaitForSeconds(healthComponent.DamageGracePeriod);

		healthComponent.TakeDamage(1.0f);
		yield return new WaitForSeconds(healthComponent.DamageGracePeriod);

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

    [UnityTest]
    public IEnumerator BasicEnemyFiresProjectile()
    {
        GameObject enemyObj = SpawnEnemy(_basicEnemyPrefab, "Enemy");
        EnemyBehaviour enemy = enemyObj.GetComponent<EnemyBehaviour>();
        yield return null;

		// Checks there's no projectiles yet.
		Projectile[] projectiles = Object.FindObjectsOfType<Projectile>();
		Assert.IsEmpty(projectiles);

		// Gets the last fire time, waits the fire interval and makes sure the
		//		new last fired time is greater than the previous.
		float lastFireTime = enemy.GetLastShotFiredTime();
		yield return new WaitForSeconds(enemy.fireInterval);
		Assert.Greater(enemy.GetLastShotFiredTime(), lastFireTime);

		// Checks there's exactly one projectile.
		projectiles = Object.FindObjectsOfType<Projectile>();
		Assert.AreEqual(1, projectiles.Length);
    }

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

