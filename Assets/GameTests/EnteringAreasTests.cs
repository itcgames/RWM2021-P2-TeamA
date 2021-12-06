using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class EnteringAreasTests
{
    // For the DoorDetectsThePlayer test.
    bool doorEntered = false;

    private void OnDoorEntered()
    {
        doorEntered = true;
    }


    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    

    [UnityTest]
    public IEnumerator DoorDetectsThePlayer()
    {
        // Gets the player object.
        var playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        // Gets the door object and script.
        var doorObj = GameObject.Find("CaveDoor");
        Assert.IsNotNull(doorObj);
        DoorDetector door = doorObj.GetComponent<DoorDetector>();
        Assert.IsNotNull(door);

        Assert.IsFalse(doorEntered);
        door.OnEnteredCallbacks.Add(OnDoorEntered);

        yield return null;
    }
}
