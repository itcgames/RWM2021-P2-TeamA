using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using TopdownCharacterController;

public class PlayerTests
{
    private CharacterBehaviour player;

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

        player = playerObj.GetComponent<CharacterBehaviour>();
        Assert.IsNotNull(player);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesCorrectly()
    {
        // Calls the previous test to get the player script and ensure it's
        //      not null before running checks on it.
        PlayerIsNotNull();

        Assert.Null(player.TilebasedMovement);
        Assert.IsFalse(player.TopdownMovement.DiagonalMovementAllowed);
        Assert.AreEqual(player.TopdownMovement.TimeToMaxSpeed, 0.0f);
        Assert.AreEqual(player.TopdownMovement.TimeToFullStop, 0.0f);

        yield return null;
    }
}

