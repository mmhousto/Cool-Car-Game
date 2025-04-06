using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentCar;

    public Player()
    {
        currentCar = 0;
    }

    public Player(SavePlayerData savePlayerData)
    {
        currentCar = savePlayerData.currentCar;
    }
}
