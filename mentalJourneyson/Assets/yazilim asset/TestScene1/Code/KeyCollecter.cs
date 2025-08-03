using System.Collections;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public bool hasKey = false;
    public int keyCount = 0; // Anahtar sayýsýný takip eden deðiþken

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            keyCount++; // Her anahtar alýndýðýnda sayacý artýr
            Destroy(other.gameObject); // Anahtar sahneden silinir
            Debug.Log($"Anahtar alýndý! Toplam: {keyCount}");
        }
    }
}
