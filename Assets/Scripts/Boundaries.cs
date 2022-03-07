using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Camera mainCamera;
    private Rect screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        Vector2 cameraSize = new Vector2(cameraWidth, cameraHeight);
        Vector2 cameraCenterPosition = mainCamera.transform.position;
        Vector2 cameraBottomLeftPos = cameraCenterPosition - (cameraSize / 2);
        screenBounds = new Rect(cameraBottomLeftPos, cameraSize);
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        screenBounds.position = (Vector2)mainCamera.transform.position - (screenBounds.size / 2);
    }

    private void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x + screenBounds.width - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y + screenBounds.height - objectHeight);
        transform.position = viewPos;
    }
}
