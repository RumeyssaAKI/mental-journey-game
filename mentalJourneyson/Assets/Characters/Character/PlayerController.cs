using UnityEngine;

// Gerekli bileþenlerin objede olmasýný zorunlu kýlar.
// Ayný anda birden fazla CharacterController olmamasý için RequireComponent eklenmedi.
// Animator da manuel olarak atanmalý.
public class PlayerController : MonoBehaviour
{
    // === BÝLEÞEN REFERANSLARI ===
    private CharacterController controller;
    private Animator animator;

    [Header("Camera Reference")]
    public Transform cameraTransform; // Kamera referansý - Inspector'dan ata

    // === HAREKET DEÐÝÞKENLERÝ ===
    // Inspector üzerinden ayarlanabilir hareket hýzlarý.
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 moveDirection;

    // === ZIPLAMA VE YER ÇEKÝMÝ ===
    public float jumpForce = 3.0f;
    public float gravity = -20.0f;
    private bool isGrounded;
    private Vector3 verticalVelocity;

    // === ANÝMATÖR PARAMETRELERÝ ===
    // Animator parametrelerinin hash deðerleri performans için önbelleðe alýnýr.
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");

    void Start()
    {
        // Bileþenler baþlangýçta alýnýr. Eðer obje üzerinde yoksa hata verir.
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Eðer kamera referansý yoksa, ana kamerayý bul
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogError("Kamera referansý bulunamadý! Inspector'dan manuel olarak atayýn.");
            }
        }

        if (controller == null)
        {
            Debug.LogError("CharacterController bileþeni karakter üzerinde bulunamadý!");
            enabled = false; // Script'i devre dýþý býrak
        }
        if (animator == null)
        {
            Debug.LogError("Animator bileþeni karakter üzerinde bulunamadý!");
            enabled = false; // Script'i devre dýþý býrak
        }
    }

    void Update()
    {
        // Zemin kontrolü
        HandleGroundDetection();

        // Hareket kontrolü
        HandleMovement();

        // Zýplama kontrolü
        HandleJumping();

        // Yer çekimi uygula
        ApplyGravity();

        // Animator parametrelerini güncelle
        UpdateAnimator();
    }

    void HandleGroundDetection()
    {
        // Raycast ile zemin kontrolü
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
            // Kameranýn yönüne göre hareket yönünü hesapla
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Y bileþenini sýfýrla (sadece horizontal hareket)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Normalize et
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Hareket yönünü kameraya göre hesapla
            moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

            // Karakteri hareket yönüne döndür
            if (moveDirection.magnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // Koþma kontrolü - hýz deðerini belirle
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
        // Zýplama kontrolü
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool(isJumpingHash, true);
        }
    }

    void ApplyGravity()
    {
        // Yer çekimi uygula
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        // Hýz deðiþkenini hesapla
        float currentSpeedValue = 0f;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Animator için hýz deðerini hesapla (0-1 arasý walk, 1-2 arasý run)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeedValue = 2f; // Run animasyonu için
            }
            else
            {
                currentSpeedValue = 1f; // Walk animasyonu için
            }
        }

        // Animator parametrelerini güncelle
        animator.SetFloat(speedHash, currentSpeedValue);
        animator.SetBool(isGroundedHash, isGrounded);

        // Yere indiðinde zýplama animasyonunu sýfýrla
        if (isGrounded && animator.GetBool(isJumpingHash))
        {
            animator.SetBool(isJumpingHash, false);
        }
    }
}