using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointController : MonoBehaviour
{
    public float rotateSpeed;
    public GameObject EndScreen;
    public Camera mainCamera;
    public float MaxDist;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
        Vector2 cameraPos = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
        Vector2 difference = new Vector2(cameraPos.x - transform.position.x, cameraPos.y - transform.position.y);
        float distance = Mathf.Sqrt((Mathf.Pow(difference.x, 2f)) + Mathf.Pow(difference.y, 2f));

        if (distance <= MaxDist)
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Overworld")
            {

                SceneManager.LoadScene("Overworld2", LoadSceneMode.Single);
            }
            else
            {
                Time.timeScale = 0;
                EndScreen.SetActive(true);
            }
        }
    }
}
