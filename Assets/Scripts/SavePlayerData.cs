using System;

[Serializable]
public class SavePlayerData 
{
    public int currentCar;

    public SavePlayerData(Player player)
    {
        currentCar = player.currentCar;
    }

    public SavePlayerData()
    {
        currentCar = 0;
    }
}
