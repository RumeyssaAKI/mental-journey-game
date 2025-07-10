using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HallucinationManager : MonoBehaviour
{
    public Volume hallucinationVolume;
    public AudioSource hallucinationSound;

    public void StartHallucination()
    {
        hallucinationVolume.weight = 1f; // Görsel efekt aç
        hallucinationSound.Play();       // Ses efekti çal

        // 5 saniye sonra efektleri kapat
        Invoke(nameof(EndHallucination), 5f);
    }

    void EndHallucination()
    {
        hallucinationVolume.weight = 0f;
    }
}

