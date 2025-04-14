using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : NetworkBehaviour
{
    public static PauseManager instance;

    public GameObject pauseMenu;
    public Button resetButton;
    public bool isPaused = false;
    private bool isLeaving = false;

    private void Awake()
    {
        instance = this;
        isLeaving = false;
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
        if (isLeaving == false)
        {
            isLeaving = true;
            NetworkManager.Singleton.OnClientStopped += ReloadGame;
            if (IsSessionOwner) 
            { 
                Debug.Log("Shutting Down"); 
                ShutdownRpc();
            }
            else
            {
                Debug.Log("Disconnecting");
                NetworkManager.DisconnectClient(NetworkObject.OwnerClientId);
            }
                
        }
        
    }

    [Rpc(SendTo.Everyone)]
    public void ShutdownRpc()
    {
        NetworkManager.Singleton.OnClientStopped += ReloadGame;
        NetworkManager.Singleton.Shutdown();

    }

    public void ReloadGame(bool value)
    {
        Destroy(GameObject.Find("SessionManager"));
        Destroy(GameObject.Find("WidgetEventDispatcher"));
        Destroy(NetworkManager.Singleton.gameObject);
        Invoke(nameof(ReloadScene), 1f);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

}
