using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed = 8f;
    
    public float jumpSpeed = 28f;
    
    public float doubleJumpSpeed = 80f;

    private Rigidbody2D _body;
    private Light _light;
    private Animator _animator;
    private BoxCollider2D _feetCollider;
    private Color _color;

    private bool _canJump = true; 
    
    private bool _canDoubleJump;
    private static readonly int Running = Animator.StringToHash("Running");

    public void Start()
    {
        _light = GetComponent<Light>();
        _color = _light.color;
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
        Flicker();
    }

    private void Flicker()
    {
        _light.color = _color * Wave();
    }

    private static float Wave() { 
        const float amplitude = 0.2f;
        const float frequency = 0.01f;
        const float phase = 0.4f;
        const float baseStart = 0.5f;
        
        var x = (Time.time + phase) * frequency;
        x = x - Mathf.Floor(x);
        var y = 1f - Random.value * 2;
           
        return y * amplitude + baseStart;    
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collision with ground
        if (other.GetContact(0).normal == Vector2.up)
        {
            ResetJump();
        }
        
        // Collision with wall
        else if (other.GetContact(0).normal == Vector2.right)
        {
            ResetJump();
        }
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

    public void ResetPlayer()
    {
        
        transform.position = new Vector3(-7, 4);
    }
}
