using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    private bool isNearLadder = false;
    private Rigidbody rb;

    public float climbSpeed = 3f;
    public KeyCode climbKey = KeyCode.E; // T?rmanma tu?u

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isNearLadder && Input.GetKey(climbKey))
        {
            // T?rmanma esnas?nda yerçekimini kapat ve yukar? ç?k
            rb.useGravity = false;
            rb.velocity = new Vector3(0f, climbSpeed, 0f);
        }
        else if (isNearLadder)
        {
            // T?rmanma tu?u b?rak?ld?, yerçekimini aç, hareketi durdur
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isNearLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isNearLadder = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }
    }
}
