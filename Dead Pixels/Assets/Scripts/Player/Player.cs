using System;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8f;

    public float jumpSpeed = 28f;

    public float doubleJumpSpeed = 80f;

    public float dashSpeed = 10f;

    public float dashTime = 0.1f;

    public bool alwaysRun;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _feetCollider;


    private float _dashTime;
    private Vector2 _dashDirection = Vector2.zero;
    private bool _canJump;
    private bool _canDoubleJump;
    private bool _canDash = true;

    private static readonly int Running = Animator.StringToHash("Running");

    public Rigidbody2D Body => _body;

    public void Start()
    {
        ResetJump();
        _body = GetComponent<Rigidbody2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    public void Update()
    {
        Run();
        Jump();
        Dash();
        FlipSprite();
    }

    static int Sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }

    private void Dash()
    {
        if (_dashDirection.Equals(Vector2.zero))
        {
            if (Input.GetButton("Fire1") && _canDash)
            {
                var directionX = Sign(Input.GetAxisRaw("Horizontal"));
                var directionY = Sign(Input.GetAxisRaw("Vertical"));
                var modifier = 1f;

                if (Mathf.Abs(directionX) == Mathf.Abs(directionY))
                {
                    modifier = 0.5f;
                }

                _dashDirection = new Vector2(
                    directionX * dashSpeed * modifier,
                    directionY * dashSpeed * modifier);

                // Reset the current velocity
                _body.velocity = Vector2.zero;
                _feetCollider.sharedMaterial.friction = 0f;
                
                // Start dashing
                _dashTime = dashTime;
            }
        }
        else
        {
            if (_dashTime <= 0)
            {
                _dashDirection = Vector2.zero;
                _dashTime = dashTime;

                // Stop the dash
                _body.velocity = Vector2.zero;
                _feetCollider.sharedMaterial.friction = 0.4f;
            }
            else
            {
                if (_canDash)
                {
                    GameEventManager.TriggerEvent("Dash");
                }
                
                _dashTime -= Time.fixedDeltaTime;
                _canDash = false;
                _body.AddForce(_dashDirection, ForceMode2D.Impulse);
            }
        }
    }

    private void ResetJump()
    {
        _canJump = true;
        _canDoubleJump = false;
        _canDash = true;
    }

    private void Run()
    {
        _animator.SetBool(Running, Math.Abs(_body.velocity.x) > Mathf.Epsilon);
        var movementVelocity = Input.GetAxisRaw("Horizontal") * movementSpeed;
        _body.velocity = new Vector2(movementVelocity, _body.velocity.y);

        if (alwaysRun)
        {
            _body.velocity = new Vector2(movementSpeed, _body.velocity.y);
        }
    }

    private void OnCollisionEnter2D()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) ResetJump();
    }


    private void OnCollisionStay2D()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) _canDash = true;
    }


    private void Jump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        if (_canJump)
        {
            _canJump = false;
            _canDoubleJump = true;
            _body.AddForce(new Vector2(_body.velocity.x, jumpSpeed), ForceMode2D.Impulse);
        }
        else if (_canDoubleJump)
        {
            _canJump = false;
            _canDoubleJump = false;

            // Reset the current velocity
            _body.velocity = new Vector2(_body.velocity.x, 0f);
            _body.AddForce(new Vector2(_body.velocity.x, doubleJumpSpeed), ForceMode2D.Impulse);
        }
    }

    private void FlipSprite()
    {
        var playerHasHorizontalSpeed = Math.Abs(_body.velocity.x) > Mathf.Epsilon;
        if (!playerHasHorizontalSpeed) return;

        var movementDirection = Mathf.Sign(_body.velocity.x);
        transform.localScale = new Vector2(movementDirection, 1f);
    }

    public void ResetPlayer()
    {
        transform.localPosition = new Vector3(-7, 4);
    }
}