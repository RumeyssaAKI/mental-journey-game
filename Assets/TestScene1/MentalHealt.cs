using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentalHealth : MonoBehaviour
{
    public float maxMentalHealth = 100f;
    public float currentMentalHealth;

    public Image healthBarFill; // UI'deki ye?il bar

    void Start()
    {
        currentMentalHealth = maxMentalHealth;
        UpdateHealthBar();
    }

    public void DecreaseMentalHealth(float amount)
    {
        currentMentalHealth -= amount;
        currentMentalHealth = Mathf.Clamp(currentMentalHealth, 0f, maxMentalHealth);
        UpdateHealthBar();

        if (currentMentalHealth <= 0f)
        {
            Debug.Log("Oyuncu öldü.");
            // Ölüm animasyonu vs.
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float fillAmount = currentMentalHealth / maxMentalHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }
}

