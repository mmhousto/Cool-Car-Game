using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{

    public int health;
    public int maxHealth = 100;
    private Color startColor;
    public Renderer mainRenderer;
    private WaitForSeconds hitEffectTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitEffectTime = new WaitForSeconds(0.15f);
        if(mainRenderer == null )
            mainRenderer = GetComponent<Renderer>();
        startColor = mainRenderer.material.color;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Damage()
    {
        StartCoroutine(FlashRed());
        health--;
        if(health < 0)
            Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        mainRenderer.material.color = Color.red;
        yield return hitEffectTime;
        mainRenderer.material.color = startColor;
    }
}
