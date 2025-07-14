using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEnemy : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    public float distanceThreshold = 15f;
    public float mirrorDelay = 0.5f;

    private float timer;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        timer = mirrorDelay;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= distanceThreshold)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                // X ve Z koordinatlar?n? yans?, Y sabit kals?n (zemin seviyesi)
                Vector3 mirroredTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.position = Vector3.MoveTowards(transform.position, mirroredTarget, followSpeed * Time.deltaTime);
                timer = mirrorDelay;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MentalHealth health = other.GetComponent<MentalHealth>();
            if (health != null)
            {
                health.DecreaseMentalHealth(20f);
            }
        }
    }
}

