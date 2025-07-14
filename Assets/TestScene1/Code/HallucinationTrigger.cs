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

    private void Start()
    {
        if (postProcessVolume != null)
            postProcessVolume.enabled = false;  // Ba?ta kapal?
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
                postProcessVolume.enabled = true;  // Efekti aç
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
                postProcessVolume.enabled = false;  // Efekti kapat
        }
    }
}
