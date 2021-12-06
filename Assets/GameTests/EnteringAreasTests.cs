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
    private void OnDoorEntered() => doorEntered = true;

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

        // Attaches the callback to the door.
        Assert.IsFalse(doorEntered);
        door.OnEnteredCallbacks.Add(OnDoorEntered);

        // Move the player to the door and checks that the door has been entered.
        playerObj.transform.position = doorObj.transform.position;
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(doorEntered);
    }
}
