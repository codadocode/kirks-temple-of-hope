using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float airMoveSpeed = 0;
    [SerializeField] private float maxHorizontalSpeed = 2;
    [SerializeField] private float maxVerticalSpeed = 5;
    [SerializeField]private int life = 3;
    [SerializeField] private GameObject groundedMovementReference;
    [SerializeField] private GameObject airMovementReference;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private BoxCollider2D footTrigger;
    
    private bool canJump = true;
    private bool isJumping = false;
    private bool isMoving = false;
    private bool canMove = true;
    
    public Action OnDie;
    public Action OnSpawn;
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Quaternion oldRotation = this.transform.rotation;
            oldRotation.z = 0;
            this.transform.rotation = oldRotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = GetMovementDirection();
        bool movement = this.isJumping ? AirControl(direction, this.airMovementReference.transform) : this.GroundControl(direction);

        if (Input.GetKey(KeyCode.W))
        {
            Jump();
        }
    }

    private Vector2 movementDirection = new Vector2();
    private Vector2 GetMovementDirection()
    {
        this.movementDirection = Vector2.zero;
        float magnitudeX = this.rb.velocity.x;

        if (Input.GetKey(KeyCode.A))
        {
            if (magnitudeX > -this.maxHorizontalSpeed)
            {
                movementDirection += -Vector2.right;
            }
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            if (magnitudeX < this.maxHorizontalSpeed)
            {
                movementDirection += Vector2.right;
            }
        }
            
        return movementDirection;
    }

    private bool AirControl(Vector2 direction, Transform forceReference)
    {
        if (direction != Vector2.zero)
        {
            this.rb.AddForceAtPosition((direction * this.airMoveSpeed) * Time.fixedDeltaTime, forceReference.position, ForceMode2D.Force);
            return true;
        }

        return false;
    }

    private bool GroundControl(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            this.rb.velocity += (direction * this.moveSpeed) * Time.deltaTime;
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject collisionGameObject = col.gameObject;
        if (collisionGameObject.CompareTag("Ground"))
        {
            ChangeCanJumpState(true);
        }
    }

    private void ChangeCanJumpState(bool canJumpState)
    {
        this.canJump = canJumpState;
        this.isJumping = !canJumpState;
    }

    private void Jump()
    {
        float magnitudeY = rb.velocity.y;
        if (!this.isJumping && this.canJump)
        {
            if (magnitudeY < this.maxVerticalSpeed)
            {
                this.rb.AddForceAtPosition((this.transform.up * this.jumpForce) * Time.fixedDeltaTime, this.groundedMovementReference.transform.position);
                ChangeCanJumpState(false);
            }
        }
    }
}
