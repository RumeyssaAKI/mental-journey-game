using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHealZone : MonoBehaviour
{
    public float healAmount = 5f;
    public float healRate = 1f; // saniyede 1 iyile?tirme

    private float timer = 0f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer >= healRate)
            {
                MentalHealth health = other.GetComponent<MentalHealth>();
                if (health != null)
                {
                    health.IncreaseMentalHealth(healAmount);
                }
                timer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }
}
