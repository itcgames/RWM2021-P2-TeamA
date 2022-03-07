using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EntityManagerTests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    //[UnityTest]
    //public IEnumerator EntitiesClearOnCameraMovement()
    //{
    //    // Ensures there are items and enemies in the scene.
    //    Assert.NotZero(GameObject.FindGameObjectsWithTag("Item").Length);
    //    Assert.NotZero(GameObject.FindGameObjectsWithTag("Enemy").Length);

    //    // Gets the camera component.
    //    var cameraObj = GameObject.Find("Main Camera");
    //    CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();

    //    // Gets the player and moves it outside the camera bounds.
    //    var playerObj = GameObject.Find("Player");
    //    playerObj.transform.position = cameraObj.transform.position +
    //        Vector3.right * camSnap.HalfAreaSize.x;

    //    yield return new WaitForSeconds(0.1f);

    //    // Checks that there's no longer any items or enemies in the scene.
    //    Assert.Zero(GameObject.FindGameObjectsWithTag("Item").Length);
    //    Assert.Zero(GameObject.FindGameObjectsWithTag("Enemy").Length);
    //}

    //[UnityTest]
    //public IEnumerator PlayerFreezesOnCameraMovement()
    //{
    //    // Gets the camera component and sets the initial values.
    //    var cameraObj = GameObject.Find("Main Camera");
    //    CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();
    //    camSnap.secondsPerPan = 0.5f;

    //    // Gets the player and moves it outside the camera bounds.
    //    var playerObj = GameObject.Find("Player");
    //    playerObj.transform.position = cameraObj.transform.position +
    //        Vector3.right * camSnap.HalfAreaSize.x;

    //    // Pauses for a fraction of a second to allow the camera to start moving.
    //    yield return new WaitForSeconds(0.1f);

    //    // Gets the player position for later comparison.
    //    Vector3 previousPlayerPosition = playerObj.transform.position;

    //    // Gets the player controller and tries to move right.
    //    var playerController = playerObj.GetComponent<TopdownCharacterController>();
    //    Assert.NotNull(playerController);
    //    playerController.MoveRight(true);

    //    // Pauses for a fraction of a second to allow the player to react.
    //    yield return new WaitForSeconds(0.1f);

    //    // Checks that the player has ignored the movement input.
    //    Assert.AreEqual(previousPlayerPosition, playerObj.transform.position);
    //}

    //[UnityTest]
    //public IEnumerator EnemiesSpawnOnCameraMovement()
    //{
    //    // Gets the camera component and sets the initial values.
    //    var cameraObj = GameObject.Find("Main Camera");
    //    CameraFollowSnap camSnap = cameraObj.GetComponent<CameraFollowSnap>();
    //    camSnap.secondsPerPan = 0.5f;

    //    // Gets the player and moves it outside the camera bounds.
    //    var playerObj = GameObject.Find("Player");
    //    playerObj.transform.position = cameraObj.transform.position +
    //        Vector3.right * camSnap.HalfAreaSize.x;

    //    // Pauses for a fraction of a second to allow the camera to start moving.
    //    yield return new WaitForSeconds(0.55f);

    //    // Checks that there's spawn smoke in the room.
    //    Assert.NotZero(GameObject.FindGameObjectsWithTag("SpawnSmoke").Length);

    //    // Gets the entity manager and waits for the enemy spawn time.
    //    var entityManagerObj = GameObject.Find("EntityManager");
    //    EntityManager entityManager = entityManagerObj.GetComponent<EntityManager>();
    //    yield return new WaitForSeconds(entityManager.enemySpawnSeconds);

    //    // Checks that there's no spawn smoke and there is enemies.
    //    Assert.Zero(GameObject.FindGameObjectsWithTag("SpawnSmoke").Length);
    //    Assert.NotZero(GameObject.FindGameObjectsWithTag("Enemy").Length);
    //}
}