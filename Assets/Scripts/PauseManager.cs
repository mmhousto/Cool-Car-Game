using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public GameObject pauseMenu;
    public Button resetButton;
    public bool isPaused = false;
    private bool isLeaving = false;

    private void Awake()
    {
        instance = this;
    }

    public void PauseResume()
    {
        isPaused = !isPaused;
        if (isPaused) Pause();
        else Resume();
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void Leave()
    {
        if(isLeaving == false)
        {
            isLeaving = true;
            NetworkManager.Singleton.OnClientStopped += ReloadGame;
            NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
        }
        
    }

    public void ReloadGame(bool value)
    {
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(0);
    }

}
