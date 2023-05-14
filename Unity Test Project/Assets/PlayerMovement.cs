using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2D;

    [Header("Movement")]
    private float horizontalMovement = 0f;
    [SerializeField] private float movementVelocity;
    [Range(0, 0.3f)][SerializeField] private float movementSmoothing;
    Vector3 Velocity = Vector3.zero;
    private bool lookingRight = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private Transform groundController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool onGround;
    private bool jump = false;

    [Header("Animation")]

    private Animator animator;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * movementVelocity;
        animator.SetFloat("Horizontal", Mathf.Abs(horizontalMovement));
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapBox(groundController.position, boxDimensions, 0f, isGround);
        animator.SetBool("onGround", onGround);
        Move(horizontalMovement * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void Move(float move, bool jump)
    {
        Vector3 objectiveVelocity = new Vector2(move, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, objectiveVelocity, ref Velocity, movementSmoothing);

        if (horizontalMovement > 0 && !lookingRight)
        {
            Rotate();
        }

        else if (horizontalMovement < 0 && lookingRight)
        {
            Rotate();
        }

        if (jump && onGround)
        {
            onGround = false;
            rb2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Rotate()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundController.position, boxDimensions);
    }
}
