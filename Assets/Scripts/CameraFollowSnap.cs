using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSnap : MonoBehaviour
{
    public Vector2 HalfAreaSize { get; private set; }
    private Camera _camera = null;

    void Start()
    {
        _camera = GetComponent<Camera>();

        // Warns the developer if no camera component can be found.
        if (!_camera)
            Debug.LogWarning("CameraFollowSnap script attached to an object " +
                "without a camera component, the script will have no effect.");

        // Gets the camera size.
        float height = _camera.orthographicSize;
        HalfAreaSize = new Vector2(height * _camera.aspect, height);
    }

    void Update()
    {
        
    }
}
