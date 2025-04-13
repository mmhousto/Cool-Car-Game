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
        Invoke(nameof(LoadGarage), 0.16f);
    }

    public void LoadGarage()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
