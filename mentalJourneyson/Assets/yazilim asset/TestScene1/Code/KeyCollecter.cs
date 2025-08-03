using System.Collections;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public bool hasKey = false;
    public int keyCount = 0; // Anahtar say�s�n� takip eden de�i�ken

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            keyCount++; // Her anahtar al�nd���nda sayac� art�r
            Destroy(other.gameObject); // Anahtar sahneden silinir
            Debug.Log($"Anahtar al�nd�! Toplam: {keyCount}");
        }
    }
}
