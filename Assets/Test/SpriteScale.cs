using UnityEngine;

public class SpriteScale : MonoBehaviour
{
    private void Start()
    {
        FitSpriteToScreen();
    }

    private void FitSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Get sprite's world size
        float spriteWidth = sr.bounds.size.x;
        float spriteHeight = sr.bounds.size.y;

        // Get screen world size
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Calculate scale factors
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        // Use the smaller scale factor to preserve aspect ratio
        float scaleFactor = Mathf.Min(scaleX, scaleY);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}
