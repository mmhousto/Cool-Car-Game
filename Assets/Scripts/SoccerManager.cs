using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SoccerManager : NetworkBehaviour
{
    public GameObject ball;
    public Transform ballSpawn;
    public NetworkVariable<int> redScore = new NetworkVariable<int>();
    public NetworkVariable<int> blueScore = new NetworkVariable<int>();
    public TextMeshPro[] redScoreLabels;
    public TextMeshPro[] blueScoreLabels;
    private Rigidbody ballRb;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            redScore.Value = 0;
            blueScore.Value = 0;
            ballRb = ball.GetComponent<Rigidbody>();
        }


        redScore.OnValueChanged += OnRedStateChanged;
        blueScore.OnValueChanged += OnBlueStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        redScore.OnValueChanged -= OnRedStateChanged;
        blueScore.OnValueChanged -= OnBlueStateChanged;
    }

    public void OnRedStateChanged(int previous, int current)
    {
        foreach (var item in redScoreLabels)
        {
            item.text = current.ToString();
        }
    }

    public void OnBlueStateChanged(int previous, int current)
    {
        foreach (var item in blueScoreLabels)
        {
            item.text = current.ToString();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void RedScoredRPC()
    {
        if (IsOwner) redScore.Value++;
    }

    [Rpc(SendTo.Everyone)]
    public void BlueScoredRPC()
    {
        if (IsOwner) blueScore.Value++;
    }

    public void RespawnBall()
    {
        if (!IsOwner) return;
        ball.transform.localPosition = ballSpawn.transform.localPosition;
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}
