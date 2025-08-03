using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 currentCheckpoint;

    private int keyCount = 0;  // Anahtar say�s�n� tutan de�i�ken

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

    // ANAHTAR S�STEM�
    public void AddKey()
    {
        keyCount++;
        Debug.Log("Anahtar al�nd�! Toplam anahtar: " + keyCount);
    }

    public int GetKeyCount()
    {
        return keyCount;
    }
}

