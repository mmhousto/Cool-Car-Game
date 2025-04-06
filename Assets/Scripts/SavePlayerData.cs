using System;
using Unity.Services.Authentication;

[Serializable]
public class SavePlayerData 
{
    public string playerName;
    public int currentCar;

    public SavePlayerData(Player player)
    {
        playerName = player.PlayerName;
        currentCar = player.CurrentCar;
    }

    public SavePlayerData()
    {
        if (AuthenticationService.Instance.IsSignedIn)
            playerName = AuthenticationService.Instance.PlayerName;
        else playerName = "Player1";
        currentCar = 0;
    }
}
