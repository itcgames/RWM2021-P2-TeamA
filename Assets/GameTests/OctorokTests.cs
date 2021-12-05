using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class OctorokTests
{
	[SetUp]
	public void Setup()
	{
		SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
	}

	[UnityTest]
	public IEnumerator IsConfinedToAreaBoundary()
	{
		GameObject cameraObj = GameObject.Find("Main Camera");
		var camController = cameraObj.GetComponent<CameraFollowSnap>();

		GameObject octorokObj = SpawnOctorok();

		// Sets the octorok's boundaries.
		var behaviour = octorokObj.GetComponent<EnemyBehaviour>();
		Assert.NotNull(behaviour);
		behaviour.AreaBounds = camController.GetBounds();

		// Moves the Octorok outside the level bounds.
		octorokObj.transform.position =
			new Vector3(behaviour.AreaBounds.right + 10.0f, 0.0f);

		yield return new WaitForSeconds(0.1f);

		// Checks that the Octorok is within the bounds again.
		Assert.LessOrEqual(octorokObj.transform.position.x, 
						   behaviour.AreaBounds.right);
	}

	[UnityTest]
	public IEnumerator DiesOnHit()
	{
		GameObject octorokObj = SpawnOctorok();
		yield return new WaitForSeconds(0.1f);

		// Sets the octorok's boundaries.
		var healthComponent = octorokObj.GetComponent<Health>();
		Assert.NotNull(healthComponent);

		// Damages the Octorok, waits, checks the Octorok is dead.
		healthComponent.TakeDamage(1.0f);
		yield return new WaitForSeconds(0.1f);
		Assert.IsNull(GameObject.Find("Octorok"));
	}

	[UnityTest]
	public IEnumerator ThornsDamage()
	{
		// Gets the player and player health.
		GameObject playerObj = GameObject.Find("Player");
		Assert.IsNotNull(playerObj);
		Health playerHealth = playerObj.GetComponent<Health>();
		Assert.IsNotNull(playerHealth);
		float healthValue = playerHealth.GetHealth();

		// Spawns an Octorok and moves it to the player.
		GameObject octorok = SpawnOctorok();
		octorok.transform.position = playerObj.transform.position;

		// Waits for the collision method to be called.
		yield return new WaitForSeconds(0.1f);

		// Cancels the flash animation to speed up the test.
		playerHealth.StopAllCoroutines();

		// Checks the health has decreased.
		Assert.Less(playerHealth.GetHealth(), healthValue);
	}

	[UnityTest]
	public IEnumerator FiresProjectile()
	{
		GameObject octorokObj = SpawnOctorok();
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

	private GameObject SpawnOctorok(string name = "Octorok")
	{
		GameObject entityManagerObj = GameObject.Find("EntityManager");
		Assert.NotNull(entityManagerObj);

		EntityManager entityManager =
			entityManagerObj.GetComponent<EntityManager>();
		Assert.NotNull(entityManager);

		GameObject octorokPrefab =
			Resources.Load<GameObject>("Prefabs/OctorokEnemy");
		Assert.NotNull(octorokPrefab);

		EnemyBehaviour octorok =
			entityManager.SpawnEnemy(octorokPrefab, Vector3.zero);
		Assert.NotNull(octorok);

		octorok.name = name;

		return octorok.gameObject;
	}
}

