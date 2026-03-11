using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveX;
    public Rigidbody2D rb;

    public float speed;
    private Vector3 refVelocity;
    public float smoothValue;

    public float jumpStrength;

    public bool jump;

    public Transform groundCheckTransform;
    public float groundCheckDistance;
    public LayerMask groundLayer;

    public float coyoteTimer;
    public float coyoteTime;

    public float bufferTimer;
    public float bufferTime;

    private void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        //if (Input.GetButtonDown("Jump"))
        //{
        //    jump = true;
        //}

        if (CheckGrounded())
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            coyoteTimer = Mathf.Clamp(coyoteTimer, -1, coyoteTime);
        }

        if (Input.GetButtonDown("Jump"))
        {
            bufferTimer = bufferTime;
        }
        else
        {
            bufferTimer -= Time.deltaTime;
            bufferTimer = Mathf.Clamp(bufferTimer, -1, bufferTime);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(moveX * speed, rb.linearVelocityY);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref refVelocity, smoothValue * Time.fixedDeltaTime);

        TryJump();
    }

    private void TryJump()
    {
        print(coyoteTimer);
        if (bufferTimer > 0 && coyoteTimer > 0)
        {
            bufferTimer = 0;
            coyoteTimer = 0;

            rb.linearVelocityY = 0;

            Vector3 jumpForce = new Vector3(0, jumpStrength);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            jump = false;
        }
    }

    private bool CheckGrounded()
    {
        bool grounded = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckDistance, groundLayer);
        //print(grounded);
        return grounded;
    }
}

