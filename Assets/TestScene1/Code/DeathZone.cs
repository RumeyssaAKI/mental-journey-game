using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public enum ZoneType { InstantKill, DamageOverTime }
    public ZoneType zoneType = ZoneType.InstantKill;

    public float damageAmount = 10f;
    public float damageRate = 1f; // Hasar uygulama aral??? (saniye)

    private float damageTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MentalHealth health = other.GetComponent<MentalHealth>();
        if (health == null) return;

        if (zoneType == ZoneType.InstantKill)
        {
            health.DecreaseMentalHealth(health.maxMentalHealth); // An?nda öldür
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MentalHealth health = other.GetComponent<MentalHealth>();
        if (health == null || zoneType != ZoneType.DamageOverTime) return;

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageRate)
        {
            health.DecreaseMentalHealth(damageAmount);
            damageTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            damageTimer = 0f; // Bölgeden ç?k?nca sayaç s?f?rlan?r
        }
    }
}

