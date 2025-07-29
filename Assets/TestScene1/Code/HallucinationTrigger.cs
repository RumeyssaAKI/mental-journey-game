using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HallucinationTrigger : MonoBehaviour
{
    public GameObject hallucinationObjects;
    public GameObject realWorldObjects;
    public Volume postProcessVolume;

    public GameObject mirrorEnemyPrefab;
    private GameObject spawnedEnemy;

    public Transform player;

    private Collider triggerCollider;

    private void Start()
    {
        if (postProcessVolume != null)
            postProcessVolume.enabled = false;

        triggerCollider = GetComponent<Collider>();  // Trigger collider referans?
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hallucinationObjects != null)
                hallucinationObjects.SetActive(true);

            if (realWorldObjects != null)
                realWorldObjects.SetActive(false);

            if (postProcessVolume != null)
                postProcessVolume.enabled = true;

            if (spawnedEnemy == null && mirrorEnemyPrefab != null && player != null)
            {
                Vector3 spawnPos = player.position - player.forward * 5f;
                spawnedEnemy = Instantiate(mirrorEnemyPrefab, spawnPos, Quaternion.identity);

                MirrorEnemy enemyScript = spawnedEnemy.GetComponent<MirrorEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.player = player;
                    enemyScript.movementBoundary = triggerCollider;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hallucinationObjects != null)
                hallucinationObjects.SetActive(false);

            if (realWorldObjects != null)
                realWorldObjects.SetActive(true);

            if (postProcessVolume != null)
                postProcessVolume.enabled = false;

            if (spawnedEnemy != null)
            {
                Destroy(spawnedEnemy);
                spawnedEnemy = null;
            }
        }
    }
}
