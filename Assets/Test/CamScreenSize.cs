using UnityEngine;

public class CamScreenSize : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Camera>().orthographicSize = Screen.height / 2;
    }

}
