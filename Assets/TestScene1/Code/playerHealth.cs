using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private PlayerRespawn respawn;

    void Start()
    {
        currentHealth = maxHealth;
        respawn = GetComponent<PlayerRespawn>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü, respawn yap?l?yor.");
        respawn.Respawn();
        currentHealth = maxHealth; // Sa?l?k s?f?rlanabilir
    }
}
