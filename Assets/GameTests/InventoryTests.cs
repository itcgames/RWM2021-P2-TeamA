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
        TestInventoryPlayer testInventory;
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
            Assert.IsNotNull(playerObj);

            testInventory = playerObj.GetComponent<TestInventoryPlayer>();
            showPanel = playerObj.GetComponent<ShowPanel>();
            Assert.IsNotNull(testInventory);
            Assert.IsNotNull(showPanel);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SpawnerIsNotNull()
        {
            PlayerIsNotNull();
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
            GameObject item = Resources.Load<GameObject>("Prefabs/DefaultItem");
            playerObj = GameObject.Find("Player");
            testInventory = playerObj.GetComponent<TestInventoryPlayer>();
            showPanel = playerObj.GetComponent<ShowPanel>();
            int amountOfItems = (int)((testInventory._maxItemsPerColumn * testInventory._maxItemsPerRow) * 1.5);
            Assert.IsNotNull(item);
            for (int i = 0; i < amountOfItems; i++)
            {
                testInventory.AddObjectToInventory(item, "Bomb", "item");
            }
            Assert.AreEqual(0, showPanel._currentlySelectedPage);
            Assert.AreEqual(0, showPanel.CurrentIndex);
            Assert.AreEqual(2, showPanel.TotalPages());
            Assert.AreEqual(amountOfItems, showPanel.TotalItems());
            testInventory.MoveLeftInInventory();
            Assert.AreEqual(0, showPanel.CurrentIndex);
            Assert.AreEqual(0, showPanel._currentlySelectedPage);
            testInventory.MoveRightInInventory();
            Assert.AreEqual(1, showPanel.CurrentIndex);
            testInventory.MoveLeftInInventory();
            Assert.AreEqual(0, showPanel.CurrentIndex);
            testInventory.MoveDownInInventory();
            Assert.AreEqual(testInventory._maxItemsPerRow, showPanel.CurrentIndex);
            testInventory.MoveUpInInventory();
            Assert.AreEqual(0, showPanel.CurrentIndex);
            for(int i = 0; i < testInventory._maxItemsPerRow; i++)
            {
                testInventory.MoveRightInInventory();
            }
            Assert.AreEqual(1, showPanel._currentlySelectedPage);
            Assert.AreEqual(0, showPanel.CurrentIndex);
            testInventory.MoveDownInInventory();
            Assert.AreEqual(0, showPanel.CurrentIndex);
            yield return null;
        }
    }
}
