using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform Goat;     // Player
    public float fixedX = 0f;     // Camera X position
    public float zOffset = -10f;  // Keep camera in front of scene

    void LateUpdate()
    {
        if (Goat == null) return;

        transform.position = new Vector3(
            fixedX,
            Goat.position.y,
            zOffset
        );
    }
}
