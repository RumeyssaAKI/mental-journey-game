using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableInteraction : MonoBehaviour
{
    public float interactDistance = 2f;
    public LayerMask pushableLayer;
    public Transform pushPoint;

    private FixedJoint joint;
    private Rigidbody currentBox;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (currentBox == null)
                TryAttachBox();
            else
                DetachBox();
        }
    }

    void TryAttachBox()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance, pushableLayer);

        if (hits.Length > 0)
        {
            Rigidbody boxRb = hits[0].attachedRigidbody;
            if (boxRb != null)
            {
                currentBox = boxRb;

                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = currentBox;
                joint.autoConfigureConnectedAnchor = true;

                Debug.Log("?? Kutuya ba?land?.");
            }
        }
    }

    void DetachBox()
    {
        if (joint != null)
            Destroy(joint);

        currentBox = null;
        Debug.Log("? Kutu ba?lant?s? kesildi.");
    }
}
