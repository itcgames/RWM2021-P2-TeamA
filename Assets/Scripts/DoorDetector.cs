using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public delegate void OnEntered();

    public List<OnEntered> OnEnteredCallbacks { get; set; }

    public CrossFade crossFade;
    public EntityManager entityManager;

    public string levelTo;

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

        // Disables the level to switch to if it exists.
        GameObject level = GameObject.Find(levelTo);
        if (level) level.SetActive(false);
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

            // TODO: Add scene switching functionality.

            crossFade.FadeFromBlack();
        }
        else if (entityManager)
            entityManager.UnpausePlayer();
    }
}
