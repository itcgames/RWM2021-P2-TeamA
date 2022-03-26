using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class AsteroidTests
    {

        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
            List<GameObject> asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();
        }

        [UnityTest]
        public IEnumerator SpawnerIsNotNull()
        {
            Spawner spawner = GetSpawner();
            Assert.IsNotNull(spawner);
            spawner.useCoRoutine = false;
            yield return null;
        }

        [UnityTest]
        public IEnumerator SpawnerCreatesCorrectAsteroid()
        {
            Spawner spawner = GetSpawner();
            spawner.useCoRoutine = false;
            List<GameObject> asteroids;

            spawner.Probability = 20;
            spawner.CreateObject();

            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            Assert.AreEqual(1, asteroids.Count);
            Assert.AreEqual("Large", asteroids[0].GetComponent<Asteroid>().Type);

            spawner.Probability = 40;
            spawner.CreateObject();

            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            Assert.AreEqual(2, asteroids.Count);
            Assert.AreEqual("Medium", asteroids[1].GetComponent<Asteroid>().Type);

            spawner.Probability = 80;
            spawner.CreateObject();

            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            Assert.AreEqual(3, asteroids.Count);
            Assert.AreEqual("Small", asteroids[2].GetComponent<Asteroid>().Type);

            yield return null;
        }

        private Spawner GetSpawner()
        {
            GameObject spawnerObj = GameObject.Find("AsteroidSpawner");
            Assert.IsNotNull(spawnerObj);

            Spawner spawner = spawnerObj.GetComponent<Spawner>();
            Assert.IsNotNull(spawner);

            return spawner;
        }
    }
}
