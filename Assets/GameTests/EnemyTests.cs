﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;
using TopdownCharacterController;

public class EnemyTests
{
	private GameObject _octorokPrefab;
	private GameObject _moblinPrefab;

	[SetUp]
	public void Setup()
	{
		_octorokPrefab = Resources.Load<GameObject>("Prefabs/OctorokEnemy");
		_moblinPrefab = Resources.Load<GameObject>("Prefabs/MoblinEnemy");
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

    private CameraFollowSnap.Bounds GetAreaBounds()
    {
        GameObject cameraObj = GameObject.Find("Main Camera");
        var camController = cameraObj.GetComponent<CameraFollowSnap>();
        return camController.GetBounds(); ;
    }

    [UnityTest]
	public IEnumerator OctorokDiesOnHit()
	{
		GameObject octorokObj = SpawnEnemy(_octorokPrefab, "TestOctorok");
		string name = octorokObj.name;
		yield return null;

		// Gets the Octorok's health component
		var healthComponent = octorokObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the Octorok, waits, checks the Octorok is dead.
		healthComponent.TakeDamage(1.0f);
		yield return null;
		Assert.IsNull(GameObject.Find(name));
	}

	[UnityTest]
	public IEnumerator OctorokDealsThornsDamage()
	{
		Health playerHealth = GetPlayersHealthComponent();
		float healthValue = playerHealth.HP;

		// Spawns an Octorok and moves it to the player.
		GameObject octorok = SpawnEnemy(_octorokPrefab, "Octorok");
		octorok.transform.position = playerHealth.transform.position;

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
	public IEnumerator OctorokFiresProjectile()
	{
		GameObject octorokObj = SpawnEnemy(_octorokPrefab, "Octorok");
		EnemyBehaviour octorok = octorokObj.GetComponent<EnemyBehaviour>();

		// Waits for the octorok to be initialised.
		yield return new WaitForSeconds(0.1f);

		float timeWaited = 0.0f;
		float lastFireTime = octorok.GetLastShotFiredTime();
		bool fired = false;

		while (timeWaited < octorok.maxFireInterval)
        {
			// If the last shot fired happened after the captured time.
			if (octorok.GetLastShotFiredTime() > lastFireTime)
            {
				fired = true;
				break;
			}

			timeWaited += 1.0f;
			yield return new WaitForSeconds(1.0f);
		}

		Assert.IsTrue(fired);
	}

	private GameObject SpawnEnemy(GameObject prefab, string name)
	{
		GameObject entityManagerObj = GameObject.Find("EntityManager");
		Assert.NotNull(entityManagerObj);

		EntityManager entityManager =
			entityManagerObj.GetComponent<EntityManager>();
		Assert.NotNull(entityManager);

		EnemyBehaviour octorok =
			entityManager.SpawnEnemy(prefab, Vector3.zero);
		Assert.NotNull(octorok);

		octorok.name = name;

		return octorok.gameObject;
	}
}

