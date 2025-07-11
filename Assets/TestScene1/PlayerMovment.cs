using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public Transform groundCheck;        // Aya??n alt?ndaki nokta
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Zemin kontrolü
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("isGrounded: " + isGrounded); // DEBUG

        // Z?plama
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space tu?una bas?ld?"); // DEBUG
            if (isGrounded)
            {
                Debug.Log("Z?plama gerçekle?ti"); // DEBUG
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
            else
            {
                Debug.Log("Havadas?n, z?playamazs?n"); // DEBUG
            }
        }
    }

    void FixedUpdate()
    {
        // Hareket
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
