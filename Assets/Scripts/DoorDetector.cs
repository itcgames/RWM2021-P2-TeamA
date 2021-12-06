using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public delegate void OnEntered();

    public List<OnEntered> OnEnteredCallbacks { get; set; }

    // Whether or not this door is active on scene load.
    public bool initiallyActive = false;

    public CrossFade crossFade;
    public EntityManager entityManager;
    public Player player;
    public Vector3 movePlayerTo = Vector3.zero;

    public List<string> levelsFrom = new List<string>();
    public List<string> levelsTo = new List<string>();

    private List<GameObject> levelsToDisable = new List<GameObject>();
    private List<GameObject> levelsToEnable = new List<GameObject>();
    private bool _switching = false;

    private void Awake()
    {
        OnEnteredCallbacks = new List<OnEntered>();
    }

    private void Start()
    {
        if (crossFade)
        {
            OnEnteredCallbacks.Add(crossFade.FadeToBlack);
            crossFade.OnFadeCompleteCallbacks.Add(OnFadeEnd);
        }

        if (entityManager)
            OnEnteredCallbacks.Add(entityManager.OnAreaChanged);

        GetLevels();

        // Disables all the levels to switch to.
        if (initiallyActive)
            foreach (GameObject level in levelsToEnable)
                level.SetActive(false);
    }

    private void GetLevels()
    {
        // Gets a reference to all the levels to switch from.
        foreach (string levelFrom in levelsFrom)
        {
            GameObject level = GameObject.Find(levelFrom);
            if (level) levelsToDisable.Add(level);
        }

        // Gets a reference to all the levels to switch to.
        foreach (string levelTo in levelsTo)
        {
            GameObject level = GameObject.Find(levelTo);
            if (level) levelsToEnable.Add(level);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If not already switching and the collider is the player.
        if (!_switching && collision.CompareTag("Player"))
        {
            // Start switching and call the callbacks.
            _switching = true;
            foreach (var callback in OnEnteredCallbacks)
                callback();
        }
    }

    private void OnFadeEnd()
    {
        if (_switching)
        {
            _switching = false;
            SwitchLevelVisibilities();
            crossFade.FadeFromBlack();
        }
        else if (entityManager)
            entityManager.UnpausePlayer();
    }

    private void SwitchLevelVisibilities()
    {
        // Disables the levels from.
        foreach (GameObject level in levelsToDisable)
            level.SetActive(false);

        // Enables the levels to.
        foreach (GameObject level in levelsToEnable)
            level.SetActive(true);

        if (player) player.transform.position = movePlayerTo;
    }
}
