using UnityEngine;

public class SpriteScale : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Camera mainCamera;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;  // Get reference to the camera

        // Adjust the orthographic size of the camera based on the screen size
        float targetAspect = 1920f / 1080f;  // The target aspect ratio for the sprite (assuming 1920x1080)
        float screenAspect = (float)Screen.width / (float)Screen.height;  // Current screen aspect ratio

        // Adjust the camera's orthographic size to maintain the aspect ratio
        if (screenAspect >= targetAspect)
        {
            // Wider screen, adjust the orthographic size based on height
            mainCamera.orthographicSize = 1080f / 2f;
        }
        else
        {
            // Taller screen, adjust the orthographic size based on width
            mainCamera.orthographicSize = (1080f / 1920f) * Screen.width / 2f;
        }

        // Scale the sprite to fit within the orthographic camera view
        float spriteWidth = sprite.bounds.size.x;  // Width of the sprite in world units
        float spriteHeight = sprite.bounds.size.y; // Height of the sprite in world units
        float cameraHeight = mainCamera.orthographicSize * 2f;  // The height of the camera's view in world units

        // Scale the sprite based on camera size
        float scaleFactor = cameraHeight / spriteHeight;
        sprite.transform.localScale = new Vector3(scaleFactor * (spriteWidth / spriteHeight), scaleFactor, 1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
