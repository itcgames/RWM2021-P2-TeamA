using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class EndScreenTest
    {

        public void Setup()
        {
            SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        }

        [UnityTest]
        public IEnumerator EndScreenIsNotActive()
        {
            GameObject EndScreen = GameObject.FindGameObjectWithTag("EndGame");
            Assert.IsNull(EndScreen);
            yield return null;
        }
    }
}
