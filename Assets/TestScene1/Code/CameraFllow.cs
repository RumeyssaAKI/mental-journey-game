using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Oyuncu (takip edilecek nesne)
    public Vector3 offset = new Vector3(0, 10, -7); // Kameran?n oyuncuya göre konumu
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyona yumu?ak geçi?
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Oyuncuya bakmaya devam et
        transform.LookAt(target);
    }
}
