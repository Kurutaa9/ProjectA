using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    public float smoothTime = 0.15f; // smaller = snappier
    public float tiltDegrees = 70f;  // 70–80 looks Archero-ish

    [Header("Dead-zone (world units, along Z)")]
    public float bottomMargin = 4f;  // how far the player can go "down" before camera moves
    public float topMargin    = 10f; // how far the player can go "up"   before camera moves

    [Header("Initial framing (taken from Scene view)")]
    public bool lockX = true;
    public bool lockY = true;

    float startX, startY;
    float zVelocity; // for SmoothDamp

    void Start()
    {
        if (!player) return;
        startX = transform.position.x;
        startY = transform.position.y;
    }

    void LateUpdate()
    {
        if (!player) return;

        // Keep your scene-view framing
        float x = lockX ? startX : transform.position.x;
        float y = lockY ? startY : transform.position.y;
        float z = transform.position.z;

        // Dead-zone edges relative to the camera
        float zBottom = z - bottomMargin; // bottom edge (closer to the player’s feet)
        float zTop    = z + topMargin;    // top edge   (ahead, where we want to see more)

        // If player leaves the window, slide the camera so they’re back on the edge
        float targetZ = z;
        if (player.position.z > zTop)
            targetZ = player.position.z - topMargin;
        else if (player.position.z < zBottom)
            targetZ = player.position.z + bottomMargin;

        // Smooth move only on Z
        float newZ = Mathf.SmoothDamp(z, targetZ, ref zVelocity, smoothTime);
        transform.position = new Vector3(x, y, newZ);

        // Fixed tilt; no yaw/roll
        transform.rotation = Quaternion.Euler(tiltDegrees, 0f, 0f);
    }
}
