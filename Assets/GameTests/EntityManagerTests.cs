using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EntityManagerTests
{
    private const string ENTITY_MANAGER_NAME = "EntityManager";

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator EnemiesSpawnOverTime()
    {
        EntityManager entityManager = GetEntityManager();

        // Asserts there are no enemies yet.
        EnemyBehaviour[] enemies = Object.FindObjectsOfType<EnemyBehaviour>();
        Assert.IsEmpty(enemies);

        // Waits the length of the spawn delay and ensures there are now some enemies.
        yield return new WaitForSeconds(entityManager.enemySpawnDelay);
        enemies = Object.FindObjectsOfType<EnemyBehaviour>();
        Assert.IsNotEmpty(enemies);
    }

    [UnityTest]
    public IEnumerator EnemySpawnRateIncreasesOverTime()
    {
        EntityManager entityManager = GetEntityManager();
        float spawnDelay = entityManager.enemySpawnDelay;

        // Waits a second and checks the spawn delay has decreased.
        yield return new WaitForSeconds(1.0f);
        Assert.Less(entityManager.enemySpawnDelay, spawnDelay);
    }

    [UnityTest]
    public IEnumerator CorrectNumberOfEnemiesSpawn()
    {
        EntityManager entityManager = GetEntityManager();

        // Asserts there are no enemies yet.
        EnemyBehaviour[] enemies = Object.FindObjectsOfType<EnemyBehaviour>();
        Assert.IsEmpty(enemies);

        // Waits the length of the spawn delay and ensures there are now some enemies.
        yield return new WaitForSeconds(entityManager.enemySpawnDelay);
        enemies = Object.FindObjectsOfType<EnemyBehaviour>();
        Assert.IsNotEmpty(enemies);

        // Checks the number of enemies is within the correct range.
        Assert.LessOrEqual(enemies.Length, entityManager.maxEnemiesPerSpawn);
        Assert.GreaterOrEqual(enemies.Length, entityManager.minEnemiesPerSpawn);
    }

    private EntityManager GetEntityManager()
    {
        GameObject entityManagerObj = GameObject.Find(ENTITY_MANAGER_NAME);
        Assert.NotNull(entityManagerObj);
        EntityManager entityManager = entityManagerObj.GetComponent<EntityManager>();
        Assert.NotNull(entityManager);
        return entityManager;
    }
}