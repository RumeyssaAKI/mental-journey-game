using System.Collections;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public bool hasKey = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject); // Anahtar sahneden silinir
            Debug.Log("Anahtar al?nd?!");
        }
    }
}
