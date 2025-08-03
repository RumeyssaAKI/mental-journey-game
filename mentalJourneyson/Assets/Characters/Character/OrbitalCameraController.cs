using UnityEngine;

public class OrbitalCameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Oyuncu karakteri
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Karakterin hangi noktas�n� takip edece�i

    [Header("Camera Distance")]
    public float distance = 5.0f; // Kameradan karaktere uzakl�k
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;
    public float zoomSpeed = 2.0f;

    [Header("Rotation Settings")]
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -30f; // A�a�� bakma limiti
    public float maxVerticalAngle = 60f;  // Yukar� bakma limiti

    [Header("Smoothing")]
    public float rotationSmoothness = 5.0f;
    public float movementSmoothness = 5.0f;

    // Private variables
    private float horizontalAngle = 0f; // Y ekseni etraf�nda d�nme (sa�-sol)
    private float verticalAngle = 20f;   // X ekseni etraf�nda d�nme (yukar�-a�a��)

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    void Start()
    {
        // E�er target atanmam��sa, oyuncu objesini bulmaya �al��
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogError("Camera target bulunamad�! Player tag'li obje var m�?");
        }

        // Ba�lang�� pozisyonu ve rotasyonu ayarla
        if (target != null)
        {
            // Kameran�n ba�lang�� a��s�n� hesapla
            Vector3 direction = transform.position - (target.position + offset);
            horizontalAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            verticalAngle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;

            distance = direction.magnitude;

            // �lk pozisyonu hesapla
            CalculateCameraPosition();
        }

        // Mouse cursor'unu kilitle (oyun modunda)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleInput();
        CalculateCameraPosition();
        ApplyCameraTransform();
    }

    void HandleInput()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (Y ekseni - sa�/sol)
        horizontalAngle += mouseX;

        // Vertical rotation (X ekseni - yukar�/a�a��) - ters �eviriyoruz ��nk� mouse Y ekseninde yukar� pozitif
        verticalAngle -= mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        // Zoom (Mouse Scroll Wheel)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // ESC tu�u ile mouse cursor'unu serbest b�rak
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Mouse t�klama ile tekrar kilitle
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void CalculateCameraPosition()
    {
        // Hedef nokta (karakterin offset'li pozisyonu)
        Vector3 targetPosition = target.position + offset;

        // Spherical coordinates'den Cartesian coordinates'e �evir
        float horizontalRadians = horizontalAngle * Mathf.Deg2Rad;
        float verticalRadians = verticalAngle * Mathf.Deg2Rad;

        // Kameran�n hedef pozisyonu
        Vector3 desiredPosition = targetPosition + new Vector3(
            Mathf.Sin(horizontalRadians) * Mathf.Cos(verticalRadians),
            Mathf.Sin(verticalRadians),
            Mathf.Cos(horizontalRadians) * Mathf.Cos(verticalRadians)
        ) * distance;

        // Smooth movement
        currentPosition = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * movementSmoothness);

        // Kameran�n bak�� y�n�
        Vector3 lookDirection = (targetPosition - currentPosition).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection);

        // Smooth rotation
        currentRotation = Quaternion.Slerp(currentRotation, desiredRotation, Time.deltaTime * rotationSmoothness);
    }

    void ApplyCameraTransform()
    {
        transform.position = currentPosition;
        transform.rotation = currentRotation;
    }

    // Debug i�in gizmos �iz
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.position + offset, 0.2f); // Target point

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position + offset, minDistance); // Min distance

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(target.position + offset, maxDistance); // Max distance

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position + offset); // Camera to target line
        }
    }
}