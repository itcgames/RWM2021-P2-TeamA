using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class EndPointTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EndsGameOnCollision()
        {
            var endPointObj = GameObject.Find("End Point");
            endPointObj.transform.position = new Vector3(5,0,0);

            var playerObj = GameObject.Find("Player");

            var gameState = UnityEditor.EditorApplication.isPlaying;

            yield return new WaitForSeconds(0.5f);
            playerObj.transform.position = endPointObj.transform.position;

            yield return new WaitForSeconds(0.5f);
            Assert.AreNotEqual(gameState, UnityEditor.EditorApplication.isPlaying);
        }
    }
}
