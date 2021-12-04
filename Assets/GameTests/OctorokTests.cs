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

        // Spawns a new Octorok.
        GameObject octorokPrefab = 
            Resources.Load<GameObject>("Prefabs/OctorokEnemy");
        Assert.NotNull(octorokPrefab);

        GameObject octorokObj = Object.Instantiate(octorokPrefab);
        Assert.NotNull(octorokObj);

        // Sets the octorok's boundaries.
        var behaviour = octorokObj.GetComponent<OctorokBehaviour>();
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
}

