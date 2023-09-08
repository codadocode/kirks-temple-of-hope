using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class PlayerMovementModule : MovementModule
{
    [Header("Configuration")] 
    [SerializeField] protected float acceleration = 5;
    [SerializeField] protected float maxHorizontalSpeed = 100;
    [SerializeField] protected float airRotationForce = 100;
    [SerializeField] protected float jumpForce = 10;
    [SerializeField] protected PlatformControllableModule controllableModule;
    [SerializeField] protected PlayableDirector director;
    [SerializeField] protected CollisionModule collisionModule;
    [SerializeField] protected Rigidbody2D rb;
    [Header("Colliders configuration")]
    [SerializeField] protected Collider2D jumpResetCollider2D;

    [Header("Pivot configuration")]
    [SerializeField] protected Transform airRotationPivot;
    protected float horizontalAxis;
    protected Vector3 lastRbVelocity;
    protected Vector3 airDirection;
    
    public float HorizontalAxis => this.horizontalAxis;

    protected override void Awake()
    {
        base.Awake();
        this.controllableModule.EvtOnMove += Move;
        this.controllableModule.EvtOnCancelMove += CancelMove;
        this.controllableModule.EvtOnJump += Jump;
        this.controllableModule.EvtOnCancelJump += CancelJump;
        this.collisionModule.EvtTriggerEnter2D += CollisionModuleOnEvtTriggerEnter2D;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.rb.rotation = 0;
            Vector2 position = this.rb.position;
            position.y -= 0.5f;
            this.rb.position = position;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.controllableModule.EvtOnMove -= Move;
        this.controllableModule.EvtOnCancelMove -= CancelMove;
        this.controllableModule.EvtOnJump -= Jump;
        this.controllableModule.EvtOnCancelJump -= CancelJump;
        this.collisionModule.EvtTriggerEnter2D -= CollisionModuleOnEvtTriggerEnter2D;
    }

    private void CollisionModuleOnEvtTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag.Equals("Ground") && obj.IsTouching(this.jumpResetCollider2D)) ResetJump();
    }

    private void ResetJump()
    {
        if (!this.jumping) return;
        this.jumping = false;
        this.lastRbVelocity = Vector2.zero;
    }

    protected override void Move()
    {
        base.Move();
        this.character.State = CHARACTER_STATE.MOVE;

        if (this.character.State != CHARACTER_STATE.MOVE) return;

        this.moving = true;
        this.horizontalAxis = this.controllableModule.HorizontalAxis;
        FlipCharacter();
        this.director.Play();
    }

    private void FlipCharacter()
    {
        Vector3 localScale = this.character.gameObject.transform.localScale;
        if (this.horizontalAxis > 0) localScale.x = 1;
        else if (this.horizontalAxis < 0) localScale.x = -1;

        this.character.gameObject.transform.localScale = localScale;
    }

    protected override void CancelMove()
    {
        base.CancelMove();
        if (this.character.State != CHARACTER_STATE.MOVE) return;

        this.character.State = CHARACTER_STATE.IDLE;
        this.moving = false;

        if (!jumping)
        {
            this.lastRbVelocity = this.rb.velocity;
            this.rb.velocity = Vector2.zero;
        }
        
        this.director.Stop();
        this.director.time = 0;
        this.director.Evaluate();
    }

    protected override void CancelJump()
    {
        base.CancelJump();
    }

    protected override void Jump()
    {
        base.Jump();
        if (jumping) return;
        
        this.jumping = true;
        this.airDirection = new Vector3(this.horizontalAxis, 0, 0);
        this.rb.AddForce(Vector2.up * this.jumpForce);
        this.rb.AddForceAtPosition(this.airDirection * airRotationForce, this.rb.position);
    }

    private void ProcessGroundMovement()
    {
        if (!this.moving || this.horizontalAxis == 0) return;

        Vector3 direction = new Vector3(this.horizontalAxis, 0, 0);
        Vector2 velocity = (direction * this.acceleration * Time.deltaTime);
        Vector2 finalVelocity = this.rb.velocity + velocity;
        finalVelocity.x = Mathf.Clamp(finalVelocity.x, -this.maxHorizontalSpeed, this.maxHorizontalSpeed);
        this.rb.velocity = finalVelocity;
    }

    private void ProcessAirMovement()
    {
        this.horizontalAxis = this.controllableModule.HorizontalAxis;
        Vector3 direction = new Vector3(this.horizontalAxis, 0, 0);
        this.rb.AddForceAtPosition(10 * direction, this.airRotationPivot.position);
    }

    private void FixedUpdate()
    {
        if (!this.jumping) ProcessGroundMovement();
        else ProcessAirMovement();
    }
}
