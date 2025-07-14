using System.Collections;
using UnityEngine;

public class MentalDamageTrigger : MonoBehaviour
{
    public float damageAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        MentalHealth mentalHealth = other.GetComponent<MentalHealth>();
        if (mentalHealth != null)
        {
            mentalHealth.DecreaseMentalHealth(damageAmount);
            Debug.Log("Hasar al?nd?, mental sa?l?k: " + mentalHealth.currentMentalHealth);
        }
    }
}
