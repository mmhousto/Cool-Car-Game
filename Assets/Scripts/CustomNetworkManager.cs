using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    /*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(Player.Instance.carPrefabs[Player.Instance.CurrentCar], Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }*/
    private Cars cars;
    private bool spawnedPlayer = false;

    private void Start()
    {
        cars = GetComponent<Cars>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnDestroy()
    {
        // Since the NetworkManager can potentially be destroyed before this component, only
        // remove the subscriptions if that singleton still exists.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        //Debug.Log("Client Connected");
        if (clientId == NetworkManager.Singleton.LocalClientId && spawnedPlayer == false && IsClient)
        {
            spawnedPlayer = true;
            SpawnCarRpc(clientId);
        }

    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        //OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Disconnected);
    }

    [Rpc(SendTo.Server)]
    public void SpawnCarRpc(ulong id)
    {
        var car = Instantiate(cars.carPrefabs[Player.Instance.CurrentCar]);
        car.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        //Debug.Log("Spawned Car");

    }
}
