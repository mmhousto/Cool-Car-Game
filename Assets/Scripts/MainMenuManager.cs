using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;

    public GameObject sessionButtons, sessionList;

    private void Awake()
    {
        instance = this;
    }

    public void DisableButtons()
    {
        sessionButtons.SetActive(false);
        sessionList.SetActive(false);
    }

    public void ToGarage()
    {
        SceneManager.LoadScene(1);
    }
}
