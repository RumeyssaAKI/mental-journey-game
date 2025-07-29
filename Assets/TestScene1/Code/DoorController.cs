using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public GameObject door;
    public KeyCollector keyCollector;
    public KeyCode openKey = KeyCode.K;
    public float closeDelay = 3f; // Kaç saniye sonra kapanacak

    private bool isPlayerNear = false;
    private bool isDoorOpen = false;

    void Update()
    {
        if (isPlayerNear && keyCollector.hasKey && Input.GetKeyDown(openKey))
        {
            if (!isDoorOpen)
                OpenDoor();
        }
    }

    void OpenDoor()
    {
        door.SetActive(false); // Kap?y? aç
        isDoorOpen = true;
        Debug.Log("Kap? aç?ld?.");

        // Belirli süre sonra kap?y? kapat
        Invoke("CloseDoor", closeDelay);
    }

    void CloseDoor()
    {
        door.SetActive(true); // Kap?y? kapat
        isDoorOpen = false;
        Debug.Log("Kap? kapand?.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}
