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
        var player = GetPlayer();
        Assert.IsNotNull(player);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesCorrectly()
    {
        var player = GetPlayer();

        Assert.Null(player.TilebasedMovement);
        Assert.IsTrue(player.TopdownMovement.DiagonalMovementAllowed);
        Assert.AreEqual(player.TopdownMovement.TimeToMaxSpeed, 1.0f);
        Assert.AreEqual(player.TopdownMovement.TimeToFullStop, 2.0f);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerHasNoMeleeAttack()
    {
        var player = GetPlayer();
        Assert.Null(player.MeleeAttack);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerHasRangedAttack()
    {
        var player = GetPlayer();
        Assert.NotNull(player.RangedAttack);
        yield return null;
    }

    private CharacterBehaviour GetPlayer()
    {
        var playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        var player = playerObj.GetComponent<CharacterBehaviour>();
        Assert.IsNotNull(player);

        return player;
    }
}

