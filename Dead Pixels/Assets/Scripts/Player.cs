using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public float movementSpeed = 8f;
    
    public float jumpSpeed = 28f;
    
    public float doubleJumpSpeed = 80f;
    
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _feetCollider;

    private UnityEvent _jumpEvent;
    
    private bool _canJump; 
    private bool _canDoubleJump;
    
    private static readonly int Running = Animator.StringToHash("Running");

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
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) ResetJump();
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
        else if(_canDoubleJump)
        {
            _canJump = false;
            _canDoubleJump = false;

            _jumpEvent?.Invoke();

            // Reset the current velocity
            _body.velocity = Vector2.zero;
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

    public void SetJumpEvent(UnityEvent jumpEvent)
    {
        _jumpEvent = jumpEvent;
    }
}
