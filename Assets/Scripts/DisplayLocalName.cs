using UnityEngine;
using TMPro;

public class DisplayLocalName : MonoBehaviour
{

    private TextMeshProUGUI playerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerName = GetComponent<TextMeshProUGUI>();
        playerName.text = Player.Instance.PlayerName;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerName != null && playerName.text != Player.Instance.PlayerName)
            playerName.text = Player.Instance.PlayerName;
    }
}
