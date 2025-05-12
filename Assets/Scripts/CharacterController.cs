using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    public float dashForce = 6f;
    public float dashCooldown = 1f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.4f;
    
    [Header("Attack")]
    public int minDamage = 1;
    public int maxDamage = 10;

    private float dashTimer;
    private bool isGrounded;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        moveInput = (transform.right * h + transform.forward*v).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
        {
            Dash();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        
        dashTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.velocity = new Vector3(moveVelocity.x, 0, moveVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Dash()
    {
        var dashDir = moveInput != Vector3.zero ? moveInput : transform.forward;
        
        rb.AddForce(dashDir.normalized * dashForce, ForceMode.Impulse);
        dashTimer = dashCooldown;
    }

    void Attack()
    {
        var damage = Random.Range(minDamage, maxDamage);
        print(damage);
        
        //TODO: play animation
    }
}
