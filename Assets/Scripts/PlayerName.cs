using UnityEngine;
using TMPro;
using Unity.Netcode;
using static UnityEngine.CullingGroup;
using System.Runtime.Serialization;
using Unity.Collections;

public class PlayerName : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();
    private TextMeshProUGUI playerNameLbl;
    private GameObject cam;

    private void Update()
    {
        if(cam == null || cam.activeInHierarchy == false) cam = GameObject.FindWithTag("MainCamera");

        if(cam != null)
            transform.parent.LookAt(cam.transform.position);

        if (playerNameLbl != null && playerNameLbl.text != playerName.Value.ToString())
        {
            playerNameLbl.text = playerName.Value.ToString();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        cam = GameObject.FindWithTag("MainCamera");

        playerNameLbl = GetComponent<TextMeshProUGUI>();

        if (IsOwner)
        {
            SetPlayerNameRpc();
            playerNameLbl.enabled = false;
        }

        playerName.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(FixedString32Bytes previous, FixedString32Bytes current)
    {
        playerNameLbl.text = current.ToString();
    }

    [Rpc(SendTo.Everyone)]
    public void SetPlayerNameRpc()
    {
        if (IsOwner)
            playerName.Value = Player.Instance.PlayerName;
        playerNameLbl.text = playerName.Value.ToString();
    }
}
