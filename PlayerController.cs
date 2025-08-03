using UnityEngine;

// Gerekli bile�enlerin objede olmas�n� zorunlu k�lar.
// Ayn� anda birden fazla CharacterController olmamas� i�in RequireComponent eklenmedi.
// Animator da manuel olarak atanmal�.
public class PlayerController : MonoBehaviour
{
    // === B�LE�EN REFERANSLARI ===
    private CharacterController controller;
    private Animator animator;

    // === HAREKET DE���KENLER� ===
    // Inspector �zerinden ayarlanabilir hareket h�zlar�.
    public float walkSpeed = 3.0f;
    public float runSpeed = 7.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 moveDirection;

    // === ZIPLAMA VE YER �EK�M� ===
    public float jumpForce = 8.0f;
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
        // Yerde olup olmad���n� kontrol et - �yile�tirilmi� zemin kontrol�
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.2f);

        // DEBUG: Zemin durumunu konsola yazd�r
        Debug.Log($"IsGrounded: {isGrounded}, Controller.isGrounded: {controller.isGrounded}, VerticalVelocity: {verticalVelocity.y}");

        // Zemin durumunu animator'a g�nder
        animator.SetBool(isGroundedHash, isGrounded);

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -0.5f; // Daha k���k de�er ile test
        }

        // Hareket girdilerini al
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Hareket y�n�n� hesapla
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // H�z de�i�kenini ba�lat
        float currentSpeedValue = 0f;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Karakteri hareket y�n�ne d�nd�r
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Ko�ma kontrol� - h�z de�erini belirle
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // Karakteri hareket ettir
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

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
        else
        {
            // Hareket etmiyorsa Speed'i s�f�rla (Idle animasyonu i�in)
            currentSpeedValue = 0f;
        }

        // Animator'daki Speed parametresini g�ncelle
        animator.SetFloat(speedHash, currentSpeedValue);

        // Z�plama kontrol� - GE��C�: Yer �ekimi olmadan test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Space tu�una bas�ld�!");

            // GE��C�: Yer �ekimi olmadan da z�playabilsin
            verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetBool(isJumpingHash, true);
            Debug.Log("ZIPLAMA TET�KLEND�! (Yer �ekimi olmadan test)");
        }

        // Yer �ekimi uygula
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Yere indi�inde z�plama animasyonunu s�f�rla
        if (isGrounded && animator.GetBool(isJumpingHash))
        {
            animator.SetBool(isJumpingHash, false);
        }
    }
}