using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public float pushForce = 5f;
    public float pushRange = 1.5f;
    public LayerMask pushableLayer;

    private bool isPushMode = false;

    void Update()
    {
        // M tu?u ile itme modu toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            isPushMode = !isPushMode;
            Debug.Log("Push mode: " + isPushMode);
        }

        if (isPushMode)
        {
            TryPushObject();
        }
    }

    void TryPushObject()
    {
        RaycastHit hit;

        // Karakterin önünde pushRange kadar Raycast gönder
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;  // biraz yukar?dan gönder
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pushRange, pushableLayer))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                // ?tme kuvveti uygula
                rb.AddForce(rayDirection * pushForce, ForceMode.Force);
            }
        }
    }
}
