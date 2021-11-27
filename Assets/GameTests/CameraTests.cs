using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CameraTests
{
    private CameraFollowSnap _camSnap;
    private Camera _camera;

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
        _camSnap = cameraObj.GetComponent<CameraFollowSnap>();
        _camera = cameraObj.GetComponent<Camera>();

        // Calculates half the camera size.
        float height = _camera.orthographicSize;
        Vector2 halfCameraSize = new Vector2(height * _camera.aspect, height);

        Assert.AreEqual(halfCameraSize, _camSnap.HalfAreaSize);

        yield return null;
    }
}