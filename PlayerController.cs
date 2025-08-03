using UnityEngine;

// Gerekli bileþenlerin objede olmasýný zorunlu kýlar.
// Ayný anda birden fazla CharacterController olmamasý için RequireComponent eklenmedi.
// Animator da manuel olarak atanmalý.
public class PlayerController : MonoBehaviour
{
    // === BÝLEÞEN REFERANSLARI ===
    private CharacterController controller;
    private Animator animator;

    // === HAREKET DEÐÝÞKENLERÝ ===
    // Inspector üzerinden ayarlanabilir hareket hýzlarý.
    public float walkSpeed = 3.0f;
    public float runSpeed = 7.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 moveDirection;

    // === ZIPLAMA VE YER ÇEKÝMÝ ===
    public float jumpForce = 8.0f;
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
        // Yerde olup olmadýðýný kontrol et - Ýyileþtirilmiþ zemin kontrolü
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.2f);

        // DEBUG: Zemin durumunu konsola yazdýr
        Debug.Log($"IsGrounded: {isGrounded}, Controller.isGrounded: {controller.isGrounded}, VerticalVelocity: {verticalVelocity.y}");

        // Zemin durumunu animator'a gönder
        animator.SetBool(isGroundedHash, isGrounded);

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -0.5f; // Daha küçük deðer ile test
        }

        // Hareket girdilerini al
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Hareket yönünü hesapla
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Hýz deðiþkenini baþlat
        float currentSpeedValue = 0f;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Karakteri hareket yönüne döndür
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Koþma kontrolü - hýz deðerini belirle
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // Karakteri hareket ettir
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

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
        else
        {
            // Hareket etmiyorsa Speed'i sýfýrla (Idle animasyonu için)
            currentSpeedValue = 0f;
        }

        // Animator'daki Speed parametresini güncelle
        animator.SetFloat(speedHash, currentSpeedValue);

        // Zýplama kontrolü - GEÇÝCÝ: Yer çekimi olmadan test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Space tuþuna basýldý!");

            // GEÇÝCÝ: Yer çekimi olmadan da zýplayabilsin
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool(isJumpingHash, true);
            Debug.Log("ZIPLAMA TETÝKLENDÝ! (Yer çekimi olmadan test)");
        }

        // Yer çekimi uygula
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Yere indiðinde zýplama animasyonunu sýfýrla
        if (isGrounded && animator.GetBool(isJumpingHash))
        {
            animator.SetBool(isJumpingHash, false);
        }
    }
}