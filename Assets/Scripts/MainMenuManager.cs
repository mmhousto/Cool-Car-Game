using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;

    public GameObject[] objectsToDisable;

    private void Awake()
    {
        instance = this;
    }

    public void DisableButtons()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

    }

    public void ToGarage()
    {
        SceneManager.LoadScene(1);
    }
}
