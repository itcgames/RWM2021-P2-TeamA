using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TopdownCharacterController;
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

        [UnityTest]
        public IEnumerator LargerAsteroidsCreateNewAsteroids()
        {
            Spawner spawner = GetSpawner();
            spawner.useCoRoutine = false;
            List<GameObject> asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();
            Assert.AreEqual(0, asteroids.Count);
            spawner.Probability = 20;
            spawner.CreateObject();
            yield return new WaitForSeconds(0.2f);//need to wait after creating the object to give unity a chance to go and set up the death callbacks or it wont generate the new asteroids correctly
            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();
            //large asteroid spawns a medium asteroid
            Assert.AreEqual(1, asteroids.Count);
            Assert.AreEqual("Large", asteroids[0].GetComponent<Asteroid>().Type);
            asteroids[0].GetComponent<Health>().TakeDamage(2.0f);
            yield return new WaitForSeconds(0.2f);//give unity time to actually go destroy the asteroid
            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            //medium asteroid spawns a small asteroid
            Assert.AreEqual(1, asteroids.Count);
            Assert.AreEqual("Medium", asteroids[0].GetComponent<Asteroid>().Type);
            asteroids[0].GetComponent<Health>().TakeDamage(1.0f);
            yield return new WaitForSeconds(0.2f);//give unity time to actually go destroy the asteroid
            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            //small asteroids shouldn't spawn anything
            Assert.AreEqual(1, asteroids.Count);
            Assert.AreEqual("Small", asteroids[0].GetComponent<Asteroid>().Type);
            asteroids[0].GetComponent<Health>().TakeDamage(1.0f);
            yield return new WaitForSeconds(0.2f);//give unity time to actually go destroy the asteroid
            asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            Assert.AreEqual(0, asteroids.Count);
            yield return null;
        }

        [UnityTest]
        public IEnumerator AsteroidsScreenWrap()
        {
            Camera camera = Camera.main;
            Vector3 top = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
            Vector3 bottom = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));

            Spawner spawner = GetSpawner();
            spawner.useCoRoutine = false;
            spawner.Probability = 20;
            spawner.CreateObject();
            spawner.CreateObject();
            yield return new WaitForSeconds(0.1f);//need to wait after creating the object to give unity a chance to go and set up the death callbacks or it wont generate the new asteroids correctly

            List<GameObject> asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();

            asteroids[0].transform.position = camera.ViewportToWorldPoint(top);//setting this to the top of the screen should make it wrap to the bottom as velocity given should make it move up
            asteroids[0].transform.position = new Vector3(asteroids[0].transform.position.x, asteroids[0].transform.position.y, 0);

            asteroids[1].transform.position = camera.ViewportToWorldPoint(bottom);//setting this to the bottom of the screen should make it wrap to the top as velocity given should make it move down
            asteroids[1].transform.position = new Vector3(asteroids[1].transform.position.x, asteroids[1].transform.position.y, 0);

            asteroids[0].GetComponent<Asteroid>().SetDirection(new Vector2(-1, -1));
            asteroids[1].GetComponent<Asteroid>().SetDirection(new Vector2(-1, 1));
            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(camera.WorldToViewportPoint(asteroids[0].transform.position).y < 0.5);//somewhere on the bottom of the screen
            Assert.IsTrue(camera.WorldToViewportPoint(asteroids[1].transform.position).y > 0.5);//somewhere on the top of the screen

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
