using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MirrorEnemy : MonoBehaviour
{
    public Transform player;
    public Collider movementBoundary;  // Hareket s?n?r? alan? (trigger collider)

    public float followDistance = 20f;
    public float damageAmount = 5f;
    public float damageInterval = 1f;

    private NavMeshAgent agent;
    private Renderer rend;
    private MentalHealth playerMentalHealth;

    private float damageTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();

        if (player != null)
            playerMentalHealth = player.GetComponent<MentalHealth>();

        if (rend != null)
            rend.enabled = false; // Ba?lang?�ta g�r�nmez
    }

    void Update()
    {
        if (player == null || agent == null || movementBoundary == null)
            return;

        // Oyuncu hareket s?n?r? i�inde mi?
        bool playerInBoundary = movementBoundary.bounds.Contains(player.position);

        // D�?man takip mesafesi i�inde mi?
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        bool inFollowRange = distToPlayer <= followDistance;

        if (playerInBoundary && inFollowRange)
        {
            // G�r�n�r yap
            if (rend != null && !rend.enabled)
                rend.enabled = true;

            // Oyuncuyu takip et
            agent.SetDestination(player.position);

            // Hasar verme
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                if (playerMentalHealth != null)
                {
                    playerMentalHealth.DecreaseMentalHealth(damageAmount);
                }
                damageTimer = 0f;
            }
        }
        else
        {
            // G�r�nmez yap
            if (rend != null && rend.enabled)
                rend.enabled = false;

            // Hareket s?n?r? i�inde kalmas? i�in merkeze d�n
            if (!movementBoundary.bounds.Contains(transform.position))
            {
                agent.SetDestination(movementBoundary.bounds.center);
            }
            else
            {
                agent.SetDestination(transform.position); // Dur
            }
        }
    }
}
