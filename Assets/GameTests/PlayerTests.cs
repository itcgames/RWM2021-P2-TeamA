using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using TopdownCharacterController;

public class PlayerTests
{
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

        var player = playerObj.GetComponent<CharacterBehaviour>();
        Assert.IsNotNull(player);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesCorrectly()
    {
        var playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        var player = playerObj.GetComponent<CharacterBehaviour>();
        Assert.IsNotNull(player);

        Assert.Null(player.TilebasedMovement);
        Assert.IsTrue(player.TopdownMovement.DiagonalMovementAllowed);
        Assert.AreEqual(player.TopdownMovement.TimeToMaxSpeed, 1.0f);
        Assert.AreEqual(player.TopdownMovement.TimeToFullStop, 2.0f);

        yield return null;
    }
}

