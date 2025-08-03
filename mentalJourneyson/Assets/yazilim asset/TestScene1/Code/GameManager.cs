using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 currentCheckpoint;

    private int keyCount = 0;  // Anahtar sayýsýný tutan deðiþken

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentCheckpoint = Vector3.zero;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // CHECKPOINT
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public Vector3 GetCheckpoint()
    {
        return currentCheckpoint;
    }

    // ANAHTAR SÝSTEMÝ
    public void AddKey()
    {
        keyCount++;
        Debug.Log("Anahtar alýndý! Toplam anahtar: " + keyCount);
    }

    public int GetKeyCount()
    {
        return keyCount;
    }
}

