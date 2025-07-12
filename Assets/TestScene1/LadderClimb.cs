using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 3f;
    public LayerMask climbableLayer;
    public KeyCode climbKey = KeyCode.E;

    private bool isClimbing = false;
    private Rigidbody rb;
    private Collider climbTrigger;

    // T?rmanma biti? yüksekli?i (uygun ayarla)
    public float climbTopOffset = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isClimbing)
        {
            if (Input.GetKey(climbKey))
            {
                rb.velocity = new Vector3(0, climbSpeed, 0);
            }

            // E?er karakter belirli yüksekli?i geçtiyse, t?rmanma sonland?r?l?r
            if (transform.position.y >= climbTrigger.bounds.max.y + climbTopOffset)
            {
                SnapToTop();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & climbableLayer) != 0)
        {
            isClimbing = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            climbTrigger = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == climbTrigger)
        {
            isClimbing = false;
            rb.useGravity = true;
        }
    }

    void SnapToTop()
    {
        isClimbing = false;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;

        // Karakterin pozisyonunu yüzeyin üstüne oturt
        Vector3 newPosition = new Vector3(transform.position.x, climbTrigger.bounds.max.y + 1f, transform.position.z);
        transform.position = newPosition;
    }
}
