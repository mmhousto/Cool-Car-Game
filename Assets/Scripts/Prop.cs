using UnityEngine;

public class Prop : MonoBehaviour, IPushable
{
    Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    public void Push(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
