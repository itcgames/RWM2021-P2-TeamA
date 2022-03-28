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
        PlayerBehaviour player = GetPlayer();
        Assert.IsNotNull(player);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesCorrectly()
    {
        PlayerBehaviour player = GetPlayer();

        Assert.Null(player.TilebasedMovement);
        Assert.IsTrue(player.TopdownMovement.DiagonalMovementAllowed);
        Assert.AreEqual(player.TopdownMovement.MaxSpeed, 3.5f);
        Assert.AreEqual(player.TopdownMovement.TimeToMaxSpeed, 0.3f);
        Assert.AreEqual(player.TopdownMovement.TimeToFullStop, 1.0f);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerHasNoMeleeAttack()
    {
        PlayerBehaviour player = GetPlayer();
        Assert.Null(player.MeleeAttack);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerHasRangedAttack()
    {
        PlayerBehaviour player = GetPlayer();
        Assert.NotNull(player.RangedAttack);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerFacesTheRight()
    {
        DisableEnemies();

        // Gets the necessary components.
        PlayerBehaviour player = GetPlayer();
        Assert.NotNull(player.Movement);
        Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
        Assert.NotNull(rigidbody);

        // Moves left persistently and checks if facing the correct direction
        //      after a delay.
        player.Movement.MoveLeft(true);
        yield return new WaitForSeconds(0.5f);

        float playerRot = player.transform.rotation.eulerAngles.z;
        Assert.AreEqual(0.0f, playerRot);

        // Clears the previous movement input.
        player.Movement.ClearPersistentInput();

        // Moves down persistently and checks if facing the correct direction
        //      after a delay.
        player.Movement.MoveUp(true);
        yield return new WaitForSeconds(0.5f);
        player.Movement.ClearPersistentInput();
        yield return new WaitForSeconds(0.1f);

        playerRot = player.transform.rotation.eulerAngles.z;
        Assert.AreEqual(0.0f, playerRot);
    }

    private PlayerBehaviour GetPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        Assert.IsNotNull(playerObj);

        PlayerBehaviour player = playerObj.GetComponent<PlayerBehaviour>();
        Assert.IsNotNull(player);

        return player;
    }

    private void DisableEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            enemy.SetActive(false);
    }
}

