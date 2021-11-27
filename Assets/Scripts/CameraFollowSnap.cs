using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSnap : MonoBehaviour
{
    // Public Variables.
    public Transform target;
    public Vector2 boundaryOffset = Vector2.zero;
    public float secondsPerPan = 0.5f;

    // Public Property.
    public Vector2 HalfAreaSize { get; private set; }

    // Internal Variables.
    private Vector3 _panStartPos = Vector3.zero;
    private Vector3 _panEndPos = Vector3.zero;
    private float _panStartSeconds = -1.0f;

    void Start()
    {
        Camera camera = GetComponent<Camera>();

        // Warns the developer if no camera component can be found.
        if (!camera)
            Debug.LogWarning("CameraFollowSnap script attached to an object " +
                "without a camera component, the script will have no effect.");

        // Gets the camera size.
        float height = camera.orthographicSize;
        HalfAreaSize = new Vector2(height * camera.aspect, height);
    }

    void Update()
    {
        // If neither the camera nor target are null.
        if (target && HalfAreaSize != Vector2.zero)
        {
            // If not panning.
            if (_panStartSeconds == -1.0f)
                CheckTargetBounds();
            else
                InterpolatePosition();
        }
    }

    private void CheckTargetBounds()
    {
        // Takes the positions into variables for ease of use.
        Vector3 tPos = target.position; // Target position.
        Vector3 mPos = transform.position; // My position.

        Vector3 panTo = Vector3.zero;

        // Checks the target against the bounds.
        if (tPos.x < mPos.x - HalfAreaSize.x + boundaryOffset.x) // Left bound.
            panTo.x = -1.0f;

        if (tPos.x > mPos.x + HalfAreaSize.x - boundaryOffset.x) // Right bound.
            panTo.x = 1.0f;

        if (tPos.y < mPos.y - HalfAreaSize.y + boundaryOffset.y) // Bottom bound.
            panTo.y = -1.0f;

        if (tPos.y > mPos.y + HalfAreaSize.y - boundaryOffset.y) // Top bound.
            panTo.y = 1.0f;

        // If the target was outside any of the bounds, begin panning.
        if (panTo != Vector3.zero)
            StartPanning(panTo);
    }

    private void StartPanning(Vector3 panTo)
    {
        _panStartSeconds = Time.time;
        _panStartPos = transform.position;
        _panEndPos = new Vector3(transform.position.x + panTo.x * HalfAreaSize.x * 2.0f,
                                 transform.position.y + panTo.y * HalfAreaSize.y * 2.0f,
                                 transform.position.z);
    }

    private void InterpolatePosition()
    {
        float secondsSincePanStart = Time.time - _panStartSeconds;
        float t = secondsSincePanStart / secondsPerPan;

        // Interpolates between the two points, clamping if t excedes 1.
        transform.position = Vector3.Lerp(_panStartPos, _panEndPos, t);
        
        // Ends the pan if the time excedes secondsPerPan.
        if (secondsSincePanStart >= secondsPerPan)
            _panStartSeconds = -1.0f;
    }
}
