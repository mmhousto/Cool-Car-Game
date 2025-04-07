using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    /*public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(Player.Instance.carPrefabs[Player.Instance.CurrentCar], Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }*/

    private void Start()
    {
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
        if(clientId == NetworkManager.Singleton.LocalClientId)
            Player.Instance.SpawnCarRpc();
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        //OnClientConnectionNotification?.Invoke(clientId, ConnectionStatus.Disconnected);
    }
}
