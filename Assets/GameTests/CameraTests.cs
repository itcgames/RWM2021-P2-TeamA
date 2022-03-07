using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CameraTests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator CameraSnapSizeIsCorrect()
    {
        // Gets the camera components.
        var cameraObj = GameObject.Find("Main Camera");
        CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();
        Camera camera = cameraObj.GetComponent<Camera>();

        // Calculates half the camera size.
        float height = camera.orthographicSize;
        Vector2 halfCameraSize = new Vector2(height * camera.aspect, height);

        Assert.AreEqual(halfCameraSize, camSnap.HalfAreaSize);

        yield return null;
    }

    //[UnityTest]
    //public IEnumerator CameraPansWhenTargetOutsideBounds()
    //{
    //    // Gets the camera components and sets the initial values.
    //    var cameraObj = GameObject.Find("Main Camera");
    //    CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();
    //    camSnap.secondsPerPan = 0.5f;
    //    camSnap.boundaryOffset = new Vector2(0.5f, 0.5f);

    //    // Gets the cam's current position and its destination.
    //    Vector3 camsInitialPos = cameraObj.transform.position;
    //    Vector3 camsDestination = camsInitialPos + Vector3.right *
    //        camSnap.HalfAreaSize.x * 2.0f;

    //    // Gets the player and moves it outside the camera bounds.
    //    var playerObj = GameObject.Find("Player");
    //    playerObj.transform.position = camsInitialPos +
    //        Vector3.right * camSnap.HalfAreaSize.x;

    //    // Checks that the camera begins moving but does not yet reach its
    //    //      destination.
    //    yield return new WaitForSeconds(0.25f);
    //    Assert.AreNotEqual(camsInitialPos, cameraObj.transform.position);
    //    Assert.AreNotEqual(camsDestination, cameraObj.transform.position);

    //    // Check that the camera has reached its destination.
    //    yield return new WaitForSeconds(0.3f);
    //    Assert.AreEqual(camsDestination, cameraObj.transform.position);
    //}

    //[UnityTest]
    //public IEnumerator CameraPushesTargetWhilePanning()
    //{
    //    // Gets the camera components and sets the initial values.
    //    var cameraObj = GameObject.Find("Main Camera");
    //    CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();
    //    camSnap.secondsPerPan = 0.5f;
    //    camSnap.boundaryOffset = new Vector2(0.5f, 0.5f);

    //    // Gets the player and moves it outside the camera bounds.
    //    var playerObj = GameObject.Find("Player");
    //    playerObj.transform.position = cameraObj.transform.position +
    //        Vector3.right * camSnap.HalfAreaSize.x;

    //    // Finds the ideal player position and checks that it matches after
    //    //      the pan time (plus a small margin for error).
    //    Vector3 idealPlayerPos = playerObj.transform.position +
    //        Vector3.right * camSnap.boundaryOffset.x;

    //    yield return new WaitForSeconds(0.6f);

    //    Assert.AreEqual(idealPlayerPos, playerObj.transform.position);
    //}
}