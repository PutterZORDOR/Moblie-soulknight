using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset from the player position
    public float smoothSpeed = 0.125f; // Smoothness factor for the follow movement

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position with offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate the camera's position towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}
