using UnityEngine;

// Gerekli bile�enlerin objede olmas�n� zorunlu k�lar.
// Ayn� anda birden fazla CharacterController olmamas� i�in RequireComponent eklenmedi.
// Animator da manuel olarak atanmal�.
public class PlayerController : MonoBehaviour
{
    // === B�LE�EN REFERANSLARI ===
    private CharacterController controller;
    private Animator animator;

    [Header("Camera Reference")]
    public Transform cameraTransform; // Kamera referans� - Inspector'dan ata

    // === HAREKET DE���KENLER� ===
    // Inspector �zerinden ayarlanabilir hareket h�zlar�.
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 moveDirection;

    // === ZIPLAMA VE YER �EK�M� ===
    public float jumpForce = 3.0f;
    public float gravity = -20.0f;
    private bool isGrounded;
    private Vector3 verticalVelocity;

    // === AN�MAT�R PARAMETRELER� ===
    // Animator parametrelerinin hash de�erleri performans i�in �nbelle�e al�n�r.
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");

    void Start()
    {
        // Bile�enler ba�lang��ta al�n�r. E�er obje �zerinde yoksa hata verir.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // E�er kamera referans� yoksa, ana kameray� bul
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogError("Kamera referans� bulunamad�! Inspector'dan manuel olarak atay�n.");
            }
        }

        if (controller == null)
        {
            Debug.LogError("CharacterController bile�eni karakter �zerinde bulunamad�!");
            enabled = false; // Script'i devre d��� b�rak
        }
        if (animator == null)
        {
            Debug.LogError("Animator bile�eni karakter �zerinde bulunamad�!");
            enabled = false; // Script'i devre d��� b�rak
        }
    }

    void Update()
    {
        // Zemin kontrol�
        HandleGroundDetection();

        // Hareket kontrol�
        HandleMovement();

        // Z�plama kontrol�
        HandleJumping();

        // Yer �ekimi uygula
        ApplyGravity();

        // Animator parametrelerini g�ncelle
        UpdateAnimator();
    }

    void HandleGroundDetection()
    {
        // Raycast ile zemin kontrol�
        bool groundCheck = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        if (groundCheck && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void HandleMovement()
    {
        // Hareket girdilerini al
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Input varsa hareket et
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Kameran�n y�n�ne g�re hareket y�n�n� hesapla
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Y bile�enini s�f�rla (sadece horizontal hareket)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Normalize et
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Hareket y�n�n� kameraya g�re hesapla
            moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

            // Karakteri hareket y�n�ne d�nd�r
            if (moveDirection.magnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // Ko�ma kontrol� - h�z de�erini belirle
                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

                // Karakteri hareket ettir
                controller.Move(moveDirection * currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void HandleJumping()
    {
        // Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool(isJumpingHash, true);
        }
    }

    void ApplyGravity()
    {
        // Yer �ekimi uygula
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        // H�z de�i�kenini hesapla
        float currentSpeedValue = 0f;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Animator i�in h�z de�erini hesapla (0-1 aras� walk, 1-2 aras� run)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeedValue = 2f; // Run animasyonu i�in
            }
            else
            {
                currentSpeedValue = 1f; // Walk animasyonu i�in
            }
        }

        // Animator parametrelerini g�ncelle
        animator.SetFloat(speedHash, currentSpeedValue);
        animator.SetBool(isGroundedHash, isGrounded);

        // Yere indi�inde z�plama animasyonunu s�f�rla
        if (isGrounded && animator.GetBool(isJumpingHash))
        {
            animator.SetBool(isJumpingHash, false);
        }
    }
}