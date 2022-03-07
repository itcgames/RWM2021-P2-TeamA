using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TopdownCharacterController;

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
        GameObject playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        // Gets the door object and script.
        GameObject doorObj = GameObject.Find("CaveDoor");
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

    [UnityTest]
    public IEnumerator FadesOnDoorEntered()
    {
        // Gets the player object and script.
        GameObject playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        // Gets the door object.
        GameObject doorObj = GameObject.Find("CaveDoor");
        Assert.IsNotNull(doorObj);

        // Gets the crossFade object, script, and image.
        GameObject crossFadeObj = GameObject.Find("CrossFade");
        Assert.IsNotNull(crossFadeObj);
        CrossFade crossFade = crossFadeObj.GetComponent<CrossFade>();
        Assert.IsNotNull(crossFade);
        Image crossFadeImage = crossFadeObj.GetComponent<Image>();
        Assert.IsNotNull(crossFadeImage);

        // Move the player to the door and waits for the fade time.
        playerObj.transform.position = doorObj.transform.position;
        yield return new WaitForSeconds(crossFade.secondsToFade);

        // Checks that the player controller is disabled.
        Assert.IsFalse(playerObj.activeSelf);

        // Checks the cross fade is at least nearly black.
        Assert.Greater(crossFadeImage.color.a, 0.9f);

        yield return new WaitForSeconds(crossFade.secondsToFade + 0.1f);

        // Checks that the player controller is enabled again.
        Assert.IsTrue(playerObj.activeSelf);

        // Checks the cross fade is transparent.
        Assert.AreEqual(crossFadeImage.color.a, 0.0f);
    }

    [UnityTest]
    public IEnumerator AreaVisibilitiesSwitch()
    {
        // Gets the player object and script.
        GameObject playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        // Gets the door object.
        GameObject doorObj = GameObject.Find("CaveDoor");
        Assert.IsNotNull(doorObj);

        // Gets the crossFade object, script, and image.
        GameObject crossFadeObj = GameObject.Find("CrossFade");
        Assert.IsNotNull(crossFadeObj);
        CrossFade crossFade = crossFadeObj.GetComponent<CrossFade>();
        Assert.IsNotNull(crossFade);

        // Gets the first level.
        GameObject firstLevel = GameObject.Find("Level 1");
        Assert.IsNotNull(firstLevel);
        Assert.IsTrue(firstLevel.activeSelf);

        // Move the player to the door and waits for the fade to elapse.
        playerObj.transform.position = doorObj.transform.position;
        yield return new WaitForSeconds(crossFade.secondsToFade * 2.0f + 0.1f);

        // Checks that the first level is no longer active.
        Assert.IsFalse(firstLevel.activeSelf);

        // Gets the third level (the level faded to) and checks that it's active.
        GameObject thirdLevel = GameObject.Find("Level 3");
        Assert.IsNotNull(thirdLevel);
        Assert.IsTrue(thirdLevel.activeSelf);
    }
}
