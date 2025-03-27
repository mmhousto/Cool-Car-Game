using UnityEngine;

public class SpriteRePosition : MonoBehaviour
{
    public Vector2 pivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the screen position (15px from the top-left corner)
        Vector3 screenPos = new Vector3(pivot.x, Screen.height - pivot.y, 10);

        // Convert to world space position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Set the sprite position
        transform.position = worldPos;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the screen position (15px from the top-left corner)
        Vector3 screenPos = new Vector3(pivot.x, Screen.height - pivot.y, 10);

        // Convert to world space position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Set the sprite position
        transform.position = worldPos;
    }
}
