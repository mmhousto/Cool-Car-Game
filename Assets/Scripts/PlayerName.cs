using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerName : MonoBehaviour
{
    private TextMeshProUGUI playerNameLbl;
    private GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        playerNameLbl = GetComponent<TextMeshProUGUI>();
        SetPlayerNameRpc(Player.Instance.PlayerName);
    }

    private void Update()
    {
        if(cam == null || cam.activeInHierarchy == false) cam = GameObject.FindWithTag("MainCamera");

        if(cam != null)
            transform.parent.LookAt(cam.transform.position);
    }

    [Rpc(SendTo.Everyone)]
    public void SetPlayerNameRpc(string newName)
    {
        playerNameLbl.text = newName;
    }
}
