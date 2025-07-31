using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = GameManager.instance.GetCheckpoint();

        if (spawnPoint == Vector3.zero)
        {
            // Checkpoint yoksa oyuncunun ba?lang?ç pozisyonu
            spawnPoint = transform.position;
        }

        transform.position = spawnPoint;
    }

    public void Respawn()
    {
        transform.position = GameManager.instance.GetCheckpoint();
    }
}
