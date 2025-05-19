using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    public Transform visuals;
    
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
    private float attackCooldown = 2f;

    private float dashTimer;
    private bool isGrounded;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private int attackIndex;
    private float attackTimer;
    
    
    [Header("Animations")]
    public Animator animator;
    public AttackInfo[] attacks;
    
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer >= dashCooldown)
        {
            Dash();
        }
        if (Input.GetMouseButtonDown(0) && attackTimer >= attackCooldown)
        {
            StartCoroutine(Attack());
        }

        if (transform.forward != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        dashTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        animator.SetBool("IsMoving", moveInput != Vector3.zero);
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
        dashTimer = 0;
    }

    IEnumerator Attack()
    {
        var damage = Random.Range(minDamage, maxDamage);
        print(damage);
        
        //TODO: play animation
        
        //animator.Play(attacks[Random.Range(0, attacks.Length)]); // my version
        
        animator.Play(attacks[attackIndex].name);
        yield return new WaitForSeconds(attacks[attackIndex].delay);

        if (attacks[attackIndex].vfx != null)
        {
            if (attacks[attackIndex].position != null)
            {
                Instantiate(attacks[attackIndex].vfx, attacks[attackIndex].position.position, attacks[attackIndex].position.rotation);
            }
        }

        attackIndex++;
        attackIndex %= attacks.Length;

        attackTimer = 0;
    }
}
