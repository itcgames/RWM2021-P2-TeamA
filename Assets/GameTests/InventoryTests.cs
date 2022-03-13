using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class InventoryTests
    {
        InventoryPlayer testInventory;
        ShowPanel showPanel;
        TestItemSpawner spawner;
        GameObject playerObj;
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("InventoryTestScene", LoadSceneMode.Single);
        }

        [TearDown]
        public void Teardown()
        {
            if (testInventory)
                Object.Destroy(testInventory.gameObject);
        }

        [UnityTest]
        public IEnumerator PlayerIsNotNull()
        {
            playerObj = GameObject.Find("Player");
            yield return new WaitForSeconds(0.2f);
            Assert.IsNotNull(playerObj);

            testInventory = playerObj.GetComponent<InventoryPlayer>();
            showPanel = playerObj.GetComponent<ShowPanel>();
            Assert.IsNotNull(testInventory);
            Assert.IsNotNull(showPanel);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SpawnerIsNotNull()
        {
            PlayerIsNotNull();
            yield return new WaitForSeconds(0.2f);
            var spawnerObj = GameObject.Find("ItemSpawner");
            Assert.IsNotNull(spawnerObj);
            spawner = spawnerObj.GetComponent<TestItemSpawner>();
            Assert.IsNotNull(spawner);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MoveInInventory()
        {
            PlayerIsNotNull();
            SpawnerIsNotNull();
            yield return new WaitForSeconds(0.2f);
            GameObject item = Resources.Load<GameObject>("Prefabs/DefaultItem");
            playerObj = GameObject.Find("Player");
            testInventory = playerObj.GetComponent<InventoryPlayer>();
            int amountOfItems = (int)((testInventory._maxItemsPerColumn * testInventory._maxItemsPerRow) * 1.5);
            Assert.IsNotNull(item);
            testInventory.OpenInventoryNoAnimation();
            for (int i = 0; i < amountOfItems; i++)
            {
                testInventory.AddObjectToInventory(item, "item", 1);
            }
            
            Assert.AreEqual(amountOfItems, testInventory.GetNumberOfItems());
            testInventory.MoveLeftInInventory();
            Assert.AreEqual(0, testInventory.GetCurrentIndex());
            testInventory.MoveRightInInventory();
            Assert.AreEqual(1, testInventory.GetCurrentIndex());
            testInventory.MoveLeftInInventory();
            Assert.AreEqual(0, testInventory.GetCurrentIndex());
            yield return null;
        }
    }
}
