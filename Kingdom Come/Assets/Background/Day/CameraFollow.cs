using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Optional offset to adjust camera position relative to player
    public float smoothSpeed = 0.125f;  // Adjust this for smoother or snappier movement

    void LateUpdate()
{
    // Define the target position the camera should move to
    Vector3 desiredPosition = player.position + offset;
    
    // Keep the camera's Z position fixed (e.g., Z = -10)
    desiredPosition.z = -10f;
    desiredPosition.y = .75f;

    // Smoothly interpolate between the camera's current position and the target position
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

    // Apply the new position to the camera
    transform.position = smoothedPosition;
}
}
