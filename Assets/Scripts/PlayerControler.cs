using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed = 2.5f;
    public float jumpForce = 4f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;

    //References
    private Animator _animator;
    private Rigidbody2D _rigidbody;


    //Move the player
    private bool _facingRight = true;
    private Vector2 _movement;
    private bool _isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw just give you from 0 to 1
        _movement = new Vector2(horizontalInput, 0f);
        if ((horizontalInput < 0f && _facingRight) || // moving left facing right
            (horizontalInput > 0 && !_facingRight)) { // moving right facing left
          Flip();
        }

        //is grounded?
        float horizontalVelocity = _movement.normalized.x * speed;
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (_isGrounded) {
          // force y velocity to 0 if it's grounded
          _rigidbody.velocity = new Vector2(horizontalVelocity, 0);
        }

        //User wants to jump and character is on the ground, either with space bar or left click.
        if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && _isGrounded)
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _animator.SetFloat("JumpVelocity", _rigidbody.velocity.y);

        }
        Debug.Log("velocity.Y after jump: " + _rigidbody.velocity.y);
        // creates a new vector because velocity.x is immutable it seems.
        _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) && _rigidbody.velocity.y == 0.0;
        if (_isGrounded) {
          // force y velocity to 0 if it's grounded
          _rigidbody.velocity = new Vector2(horizontalVelocity, 0);
        }
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("JumpVelocity", _rigidbody.velocity.y);
        _animator.SetBool("isFalling", _rigidbody.velocity.y < 0.0f);
        _animator.SetBool("isMoving", _movement != Vector2.zero);
    }

    //Flip player to the left or right.
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }
}
