using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointController : MonoBehaviour
{
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerBehaviour player = collision.GetComponent<PlayerBehaviour>();
            if (player) player.completed = true;

            Application.Quit();
            //if (UnityEditor.EditorApplication.isPlaying)
            //{
            //    UnityEditor.EditorApplication.isPlaying = false;
            //}
        }
    }
}
