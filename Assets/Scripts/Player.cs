using System.Data;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player instance;

    public static Player Instance { get { return instance; } }

    [SerializeField]
    private int currentCar;
    public int CurrentCar { get { return currentCar; } private set { currentCar = value; } }

    [SerializeField]
    private string playerName;
    public string PlayerName { get { return playerName; } private set { playerName = value; } }

    public GameObject[] carPrefabs;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(Instance.gameObject);
        }
    }

    private void Start()
    {
        SetData(SaveSystem.LoadPlayer());
    }

    private void Update()
    {
        if (AuthenticationService.Instance.IsSignedIn && PlayerName != AuthenticationService.Instance.PlayerName)
                PlayerName = AuthenticationService.Instance.PlayerName;
    }

    public void SpawnCar()
    {
        var car = Instantiate(carPrefabs[Player.Instance.CurrentCar]);
        car.GetComponent<NetworkObject>().Spawn();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void SetData()
    {
        if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            PlayerName = AuthenticationService.Instance.PlayerName;
        else PlayerName = "Player1";

        CurrentCar = 0;
    }

    public void SetData(SavePlayerData savePlayerData)
    {
        PlayerName = savePlayerData.playerName;
        CurrentCar = savePlayerData.currentCar;
    }

    public void UpdateCurrentCar(int newCar)
    {
        CurrentCar = newCar;
    }

    private void OnApplicationQuit()
    {
        SavePlayer();
    }
}
