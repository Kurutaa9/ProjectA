using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smooth = 5f;
    public float leadZ = 3f;          // how much to see ahead of the player

    float startX;                      // camera X to keep forever
    float startY;                      // camera height to keep forever
    float baseZOffset;                 // initial depth relative to the player

    void Start()
    {
        if (!player) return;

        // Keep the framing you set in the editor
        startX = transform.position.x;
        startY = transform.position.y;

        // Remember how far behind/ahead of the player the camera starts
        baseZOffset = transform.position.z - player.position.z;
    }

    void LateUpdate()
    {
        if (!player) return;

        // Only Z follows; X/Y stay at the starting values
        float x = startX;
        float y = startY;
        float z = player.position.z + baseZOffset + leadZ;

        Vector3 target = new Vector3(x, y, z);
        transform.position = Vector3.Lerp(transform.position, target, smooth * Time.deltaTime);

        // Slight tilt like Archero (adjust if you want more/less tilt)
        transform.rotation = Quaternion.Euler(70f, 0f, 0f);
    }
}
