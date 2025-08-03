using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HallucinationTrigger : MonoBehaviour
{
    public GameObject hallucinationObjects;   // Halüsinasyonda aktif olacak nesneler
    public GameObject realWorldObjects;       // Normal dünyada aktif olan nesneler
    public Volume postProcessVolume;           // Post-processing efekti

    public GameObject mirrorEnemyPrefab;      // Spawn edilecek düþman prefabý
    private GameObject spawnedEnemy;

    public Transform player;                   // Oyuncunun Transform'u

    public GameObject rainEffect;              // Yaðmur efekti

    private Collider triggerCollider;

    private void Start()
    {
        if (postProcessVolume != null)
            postProcessVolume.enabled = false;

        if (rainEffect != null)
            rainEffect.SetActive(false);

        triggerCollider = GetComponent<Collider>();
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

            if (rainEffect != null)
                rainEffect.SetActive(true);

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

            if (rainEffect != null)
                rainEffect.SetActive(false);

            if (spawnedEnemy != null)
            {
                Destroy(spawnedEnemy);
                spawnedEnemy = null;
            }
        }
    }
}
