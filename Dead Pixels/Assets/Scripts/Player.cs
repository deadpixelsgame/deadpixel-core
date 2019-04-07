using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{

    public float movementSpeed = 8f;
    
    public float jumpSpeed = 28f;
    
    public float doubleJumpSpeed = 80f;

    private Rigidbody2D _body;
    private BoxCollider2D _feetCollider;
    
    private Animator _animator;

    private bool _canJump = true; 
    
    private bool _canDoubleJump;
    private static readonly int Running = Animator.StringToHash("Running");

    public void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }
    

    // Start is called before the first frame update
    public void Update()
    {
        Run();
        Jump();
        FlipSprite();
    }

    private void ResetJump()
    {
        _canJump = true;
        _canDoubleJump = false;
    }
    
    private void Run()
    {
        _animator.SetBool(Running, Math.Abs(_body.velocity.x) > Mathf.Epsilon);
        var movementVelocity = Input.GetAxisRaw("Horizontal") * movementSpeed;
        _body.velocity = new Vector2(movementVelocity, _body.velocity.y);
    }
    
    private void Jump()
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) ResetJump();
        if (!Input.GetButtonDown("Jump")) return;
        
        if (_canJump)
        {      
            _body.AddForce(new Vector2(_body.velocity.x, jumpSpeed), ForceMode2D.Impulse);
            _canJump = false;
            _canDoubleJump = true;
        }
        else if(_canDoubleJump)
        {
            // Reset the current velocity
            _body.velocity = Vector2.zero;
            _body.AddForce(new Vector2(_body.velocity.x, doubleJumpSpeed), ForceMode2D.Impulse);

            _canJump = false;
            _canDoubleJump = false;
        }
    }

    private void FlipSprite()
    {
        var playerHasHorizontalSpeed = Math.Abs(_body.velocity.x) > Mathf.Epsilon;
        if (!playerHasHorizontalSpeed) return;
        
        var movementDirection = Mathf.Sign(_body.velocity.x);
        transform.localScale = new Vector2(movementDirection, 1f);
    }
}
