using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSnap : MonoBehaviour
{
    public delegate void MovementEventCallback();

    public struct Bounds
    {
        public float left;
        public float right;
        public float bottom;
        public float top;
    }

    // Public Variables.
    public Transform target;
    public Vector2 boundaryOffset = Vector2.zero;
    public float secondsPerPan = 0.5f;

    // Public Property.
    public Vector2 HalfAreaSize { get; private set; }
    public List<MovementEventCallback> BeginMovementCallbacks { get; set; }
    public List<MovementEventCallback> EndMovementCallbacks { get; set; }

    // Internal Variables.
    private Vector3 _panStartPos = Vector3.zero;
    private Vector3 _panEndPos = Vector3.zero;
    private float _panStartSeconds = -1.0f;

    private void Awake()
    {
        BeginMovementCallbacks = new List<MovementEventCallback>();
        EndMovementCallbacks = new List<MovementEventCallback>();
    }

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
        Vector3 panTo = Vector3.zero;
        Bounds bounds = GetBounds();

        // Checks the target against the bounds.
        if (target.position.x < bounds.left)
            panTo.x = -1.0f;

        else if (target.position.x > bounds.right)
            panTo.x = 1.0f;

        if (target.position.y < bounds.bottom)
            panTo.y = -1.0f;

        else if (target.position.y > bounds.top)
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

        foreach (var callback in BeginMovementCallbacks)
            callback();
    }

    private void InterpolatePosition()
    {
        float secondsSincePanStart = Time.time - _panStartSeconds;
        float t = secondsSincePanStart / secondsPerPan;

        // Interpolates between the two points, clamping if t excedes 1.
        transform.position = Vector3.Lerp(_panStartPos, _panEndPos, t);

        // Pushes the target if it's within the boundary offset.
        HandleTargetMovement();

        // Ends the pan if the time excedes secondsPerPan.
        if (secondsSincePanStart >= secondsPerPan)
        {
            _panStartSeconds = -1.0f;

            foreach (var callback in EndMovementCallbacks)
                callback();
        }
    }

    private void HandleTargetMovement()
    {
        // Takes the target's position into variables for readability.
        Vector3 tPos = target.position;

        Bounds bounds = GetBounds();

        if (tPos.x < bounds.left) 
            target.position = new Vector3(bounds.left, tPos.y, tPos.z);

        else if (tPos.x > bounds.right)
            target.position = new Vector3(bounds.right, tPos.y, tPos.z);

        if (tPos.y < bounds.bottom)
            target.position = new Vector3(tPos.x, bounds.bottom, tPos.z);

        else if (tPos.y > bounds.top)
            target.position = new Vector3(tPos.x, bounds.top, tPos.z);
    }

    public Bounds GetBounds()
    {
        return new Bounds()
        {
            left = transform.position.x - HalfAreaSize.x + boundaryOffset.x,
            right = transform.position.x + HalfAreaSize.x - boundaryOffset.x,
            bottom = transform.position.y - HalfAreaSize.y + boundaryOffset.y,
            top = transform.position.y + HalfAreaSize.y - boundaryOffset.y
        };
    }
}
