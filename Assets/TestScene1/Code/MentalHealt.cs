using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentalHealth : MonoBehaviour
{
    public float maxMentalHealth = 100f;
    public float currentMentalHealth;

    public Image healthBarFill;

    private PlayerRespawn respawn;

    void Start()
    {
        currentMentalHealth = maxMentalHealth;
        respawn = GetComponent<PlayerRespawn>();
        UpdateHealthBar();
    }

    public void DecreaseMentalHealth(float amount)
    {
        currentMentalHealth -= amount;
        currentMentalHealth = Mathf.Clamp(currentMentalHealth, 0f, maxMentalHealth);
        UpdateHealthBar();

        if (currentMentalHealth <= 0f)
        {
            Debug.Log("Mental health bitti. Respawn ediliyor.");
            if (respawn != null)
            {
                respawn.Respawn();
                currentMentalHealth = maxMentalHealth;
                UpdateHealthBar();
            }
        }
    }

    public void IncreaseMentalHealth(float amount)
    {
        currentMentalHealth += amount;
        currentMentalHealth = Mathf.Clamp(currentMentalHealth, 0f, maxMentalHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentMentalHealth / maxMentalHealth;
        }
    }
}

