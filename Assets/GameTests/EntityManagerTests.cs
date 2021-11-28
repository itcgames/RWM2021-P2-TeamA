using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EntityManagerTests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator EntitiesClearOnCameraMovement()
    {
        // Ensures there are items and enemies in the scene.
        Assert.NotZero(GameObject.FindGameObjectsWithTag("Item").Length);
        Assert.NotZero(GameObject.FindGameObjectsWithTag("Enemy").Length);

        // Gets the camera component.
        var cameraObj = GameObject.Find("Main Camera");
        CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();

        // Gets the player and moves it outside the camera bounds.
        var playerObj = GameObject.Find("Player");
        playerObj.transform.position = cameraObj.transform.position +
            Vector3.right * camSnap.HalfAreaSize.x;

        yield return new WaitForSeconds(0.1f);

        // Checks that there's no longer any items or enemies in the scene.
        Assert.Zero(GameObject.FindGameObjectsWithTag("Item").Length);
        Assert.Zero(GameObject.FindGameObjectsWithTag("Enemy").Length);
    }
}