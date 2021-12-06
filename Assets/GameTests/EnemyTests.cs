using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

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

	[UnityTest]
	public IEnumerator IsOctorokConfinedToAreaBoundary()
	{
		var areaBounds = GetAreaBounds();

		// Spawns an Octorok and moves it outside the level bounds.
		GameObject octorok = SpawnEnemy(_octorokPrefab, "Octorok");
		octorok.transform.position = new Vector3(areaBounds.right + 10.0f, 0.0f);

		// Checks that the Octorok is within the bounds again next frame.
		yield return null;
		Assert.LessOrEqual(octorok.transform.position.x, areaBounds.right);
	}

	[UnityTest]
	public IEnumerator IsMoblinConfinedToAreaBoundary()
	{
		var areaBounds = GetAreaBounds();

		// Spawns a Moblin and moves it outside the level bounds.
		GameObject moblin = SpawnEnemy(_moblinPrefab, "Moblin");
		moblin.transform.position = new Vector3(areaBounds.right + 10.0f, 0.0f);

		// Checks that the Moblin is within the bounds again next frame.
		yield return null;
		Assert.LessOrEqual(moblin.transform.position.x, areaBounds.right);
	}

	private CameraFollowSnap.Bounds GetAreaBounds()
    {
		GameObject cameraObj = GameObject.Find("Main Camera");
		var camController = cameraObj.GetComponent<CameraFollowSnap>();
		return camController.GetBounds(); ;
	}

	[UnityTest]
	public IEnumerator OctorokDiesOnHit()
	{
		GameObject octorokObj = SpawnEnemy(_octorokPrefab, "Octorok");
		yield return null;

		// Gets the Octorok's health component
		var healthComponent = octorokObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the Octorok, waits, checks the Octorok is dead.
		healthComponent.TakeDamage(1.0f);
		yield return null;
		Assert.IsNull(GameObject.Find("Octorok"));
	}

	[UnityTest]
	public IEnumerator MoblinDiesOnTwoHits()
	{
		GameObject moblinObj = SpawnEnemy(_moblinPrefab, "Moblin");
		yield return null;

		// Gets the Moblin's health component
		var healthComponent = moblinObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the Moblin, waits, checks the Moblin is dead.
		healthComponent.TakeDamage(2.0f);
		yield return null;
		Assert.IsNull(GameObject.Find("Moblin"));
	}

	[UnityTest]
	public IEnumerator OctorokDealsThornsDamage()
	{
		Health playerHealth = GetPlayersHealthComponent();
		float healthValue = playerHealth.GetHealth();

		// Spawns an Octorok and moves it to the player.
		GameObject octorok = SpawnEnemy(_octorokPrefab, "Octorok");
		octorok.transform.position = playerHealth.transform.position;

		// Waits for the collision method to be called.
		yield return new WaitForSeconds(0.1f);

		// Cancels the flash animation to speed up the test.
		playerHealth.StopAllCoroutines();

		// Checks the health has decreased.
		Assert.Less(playerHealth.GetHealth(), healthValue);
	}

	[UnityTest]
	public IEnumerator MoblinDealsThornsDamage()
	{
		Health playerHealth = GetPlayersHealthComponent();
		float healthValue = playerHealth.GetHealth();

		// Spawns a Moblin and moves it to the player.
		GameObject moblin = SpawnEnemy(_octorokPrefab, "Moblin");
		moblin.transform.position = playerHealth.transform.position;

		// Waits for the collision method to be called.
		yield return new WaitForSeconds(0.1f);

		// Cancels the flash animation to speed up the test.
		playerHealth.StopAllCoroutines();

		// Checks the health has decreased.
		Assert.Less(playerHealth.GetHealth(), healthValue);
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

	[UnityTest]
	public IEnumerator MoblinFiresProjectile()
	{
		GameObject moblinObj = SpawnEnemy(_moblinPrefab, "Moblin");
		EnemyBehaviour moblin = moblinObj.GetComponent<EnemyBehaviour>();

		// Waits for the octorok to be initialised.
		yield return new WaitForSeconds(0.1f);

		float timeWaited = 0.0f;
		float lastFireTime = moblin.GetLastShotFiredTime();
		bool fired = false;

		while (timeWaited < moblin.maxFireInterval)
		{
			// If the last shot fired happened after the captured time.
			if (moblin.GetLastShotFiredTime() > lastFireTime)
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

