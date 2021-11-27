using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerTests
{
    private TopdownCharacterController player;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator PlayerIsNotNull()
    {
        var playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        player = playerObj.GetComponent<TopdownCharacterController>();
        Assert.IsNotNull(player);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesCorrectly()
    {
        // Calls the previous test to get the player script and ensure it's
        //      not null before running checks on it.
        PlayerIsNotNull();

        Assert.IsFalse(player.TilebasedMovement);
        Assert.IsFalse(player.DiagonalMovementAllowed);
        Assert.AreEqual(player.TimeToMaxSpeed, 0.0f);
        Assert.AreEqual(player.TimeToFullStop, 0.0f);

        yield return null;
    }
}

