using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smooth speed of the camera
    public Vector3 offset = new Vector3(0, 0, -10);

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
