using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento y Rotacion")]
    public float originalSpeed = 8f;
    public float moveSpeed = 8f;
  
    public float turnSpeed = 10f;

    [Header("Deteccion suelo")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Estado (Desmayado)")]
    public bool isKnockedOut = false;
    public float recoveryTime = 3f;


    [Header("Salto")]
    public float jumpForce;      
    public float fallMultiplier = 2.5f;       
    public float lowJumpMultiplier = 5f;     
    public float maxFallSpeed = 25f;         
    public float coyoteTime = 0.1f;         
    public float jumpBufferTime = 0.1f;


    private bool jumpHeld;
    private float coyoteTimer;
    private float bufferTimer;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (isKnockedOut) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        moveInput = (camForward * z + camRight * x).normalized;

        jumpHeld = Input.GetButton("Jump");

        if (Input.GetButtonDown("Jump"))
            bufferTimer = jumpBufferTime;
        else
            bufferTimer -= Time.deltaTime;

        if (isGrounded && rb.linearVelocity.y <= 0.01f)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
       
    }

    void FixedUpdate()
    {
        if (isKnockedOut) return;

        Vector3 targetVelocity = moveInput * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }

        HandleJump();
    }


    void HandleJump()
    {
        Vector3 v = rb.linearVelocity;

     
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            v.y = jumpForce;
            bufferTimer = 0f;
            coyoteTimer = 0f;
        }

        
        if (v.y < 0f)
        {
        
            v.y += Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (v.y > 0f && !jumpHeld)
        {
         
            v.y += Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        
        if (v.y < -maxFallSpeed) v.y = -maxFallSpeed;

        rb.linearVelocity = v;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("win"))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("win"))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void HitAndFall(Vector3 impactDirection, float force)
    {
        if (isKnockedOut) return;

        isKnockedOut = true;

        rb.constraints = RigidbodyConstraints.None;

        rb.AddForce(impactDirection * force, ForceMode.Impulse);

        Invoke(nameof(Recover), recoveryTime);
    }

    void Recover()
    {
        isKnockedOut = false;

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void ChangeSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void RestoreSpeed()
    {
        moveSpeed = originalSpeed;
    }

    public void Die()
    {
        if (isKnockedOut) return;
        isKnockedOut = true;

        rb.constraints = RigidbodyConstraints.None;

        //rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        Invoke(nameof(RestartLevel), 2f);
    }

    void RestartLevel()
    {
        DeathCounter.AddDeath();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

