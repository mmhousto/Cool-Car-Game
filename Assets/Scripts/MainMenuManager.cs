using Unity.Services.Multiplayer;
using UnityEngine;

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
}
