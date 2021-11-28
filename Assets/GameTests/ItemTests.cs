using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTests
    {
        TestInventoryPlayer testInventory;
        ShowPanel showPanel;
        TestItemSpawner spawner;
        GameObject playerObj;
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("ItemTestScene", LoadSceneMode.Single);
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ItemNotSpawnedAtPlayer()
        {
            playerObj = GameObject.Find("Player");
            testInventory = playerObj.GetComponent<TestInventoryPlayer>();
            showPanel = playerObj.GetComponent<ShowPanel>();
            
            var spawnerObj = GameObject.Find("BombSpawner");
            Assert.IsNotNull(spawnerObj);
            spawner = spawnerObj.GetComponent<TestItemSpawner>();
            spawner.spawnLocations = new Transform[1];
            spawner.spawnLocations[0] = playerObj.transform;
            GameObject item = spawner.CreateItem();
            Assert.NotNull(item);
            Assert.AreNotEqual(item.transform.position, playerObj.transform.position);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ItemGoesToCorrectSpawnLocation()
        {
            playerObj = GameObject.Find("Player");
            testInventory = playerObj.GetComponent<TestInventoryPlayer>();
            showPanel = playerObj.GetComponent<ShowPanel>();
            var spawnerObj = GameObject.Find("BombSpawner");
            Assert.IsNotNull(spawnerObj);
            spawner = spawnerObj.GetComponent<TestItemSpawner>();
            yield return new WaitForSeconds(0.2f);
            GameObject item = spawner.CreateItem();
            yield return new WaitForSeconds(0.2f);
            Assert.NotNull(item);
            Assert.NotNull(spawner.spawnLocations);
            Assert.AreEqual(spawner.spawnLocations.Length, 3);
            Assert.AreEqual(item.transform.position, spawner.spawnLocations[0].position);
            spawner.spawnLocations[0] = playerObj.transform;
            item = spawner.CreateItem();
            Assert.AreNotEqual(item.transform.position, playerObj.transform.position);
            yield return null;
        }

        //[UnityTest]
        //public IEnumerator PickUpCorrectAmountOfBombs()
        //{
        //    playerObj = GameObject.Find("Player");
        //    testInventory = playerObj.GetComponent<TestInventoryPlayer>();
        //    showPanel = playerObj.GetComponent<ShowPanel>();
        //    GameObject bomb = Resources.Load<GameObject>("Prefabs/Bomb");
        //    bomb.transform.position = playerObj.transform.position;
        //    yield return new WaitForSeconds(0.2f);
        //    Assert.AreEqual(testInventory.GetAmountOfItemAtPosition(0), 4);
        //    yield return null;
        //}
    }
}
