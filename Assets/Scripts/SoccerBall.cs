using Unity.Netcode;
using UnityEngine;

public class SoccerBall : NetworkBehaviour
{
    public SoccerManager soccerManager;

    private void OnTriggerExit(Collider other)
    {
        if(IsOwner && other.CompareTag("Soccer"))
        {
            soccerManager.RespawnBall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsOwner && other.CompareTag("Blue"))
        {
            soccerManager.BlueScoredRPC();
            soccerManager.RespawnBall();
        }
        if (IsOwner && other.CompareTag("Red"))
        {
            soccerManager.RedScoredRPC();
            soccerManager.RespawnBall();
        }
    }
}
