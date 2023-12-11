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
    [SerializeField] protected float maxGroundedHorizontalVelocity = 100;
    [SerializeField] protected float maxAirHorizontalVelocity = 5;
    [SerializeField] protected float jumpForce = 10;
    [SerializeField] protected float airMovementRotationForce = 10;
    [SerializeField] protected GamePlataformControllableModule controllableModule;
    [SerializeField] protected PlayableDirector director;
    [SerializeField] protected CollisionModule collisionModule;
    [SerializeField] protected Rigidbody2D rb;
    [Header("Colliders configuration")]
    [SerializeField] protected Collider2D jumpResetCollider2D;
    [SerializeField] protected Collider2D rightCollider2DTop;
    [SerializeField] protected Collider2D rightCollider2DBottom;
    [SerializeField] protected Collider2D leftCollider2DTop;
    [SerializeField] protected Collider2D leftCollider2DBottom;
    [SerializeField] protected Collider2D headLeftColider2D;
    [SerializeField] protected Collider2D headRightColider2D;
    [Header("UI Configuration")]
    [SerializeField] protected FillBarManager fillBarManager;
    [SerializeField] protected float turnUpValueToAdd = 5f;

    [Header("Pivot configuration")]
    [SerializeField] protected Transform airRotationPivot;
    protected float horizontalAxis;
    protected Vector3 lastRbVelocity;
    protected Vector3 airDirection;
    public bool stuck = false;
    
    public float HorizontalAxis => this.horizontalAxis;

    protected override void Awake()
    {
        base.Awake();
        this.controllableModule.EvtOnMove += Move;
        this.controllableModule.EvtOnCancelMove += CancelMove;
        this.controllableModule.EvtOnJump += Jump;
        this.controllableModule.EvtOnCancelJump += CancelJump;
        this.controllableModule.EvtOnTurnUp += TurnUp;
        this.controllableModule.EvtOnCancelTurnUp += CancelTurnUp;
        this.collisionModule.EvtTriggerEnter2D += CollisionModuleOnEvtTriggerEnter2D;
        this.collisionModule.EvtTriggerExit2D += CollisionModuleOnEvtTriggerExit2D;
        this.fillBarManager.OnBarCompleteEvent += Unstuck;
    }

    private void CancelTurnUp()
    {
        //throw new NotImplementedException();
    }

    private void TurnUp()
    {
        this.fillBarManager.Interact(turnUpValueToAdd);
    }

    private void Unstuck()
    {
        this.rb.rotation = 0;
        Vector2 position = this.rb.position;
        position.y += 0.5f;
        this.rb.position = position;
    }

    private void CollisionModuleOnEvtTriggerExit2D(Collider2D obj)
    {
        string objTag = obj.gameObject.tag;
        switch (objTag)
        {
            case "Ground":
                if (!obj.IsTouching(this.jumpResetCollider2D))
                {
                    ToggleGroundState(false);
                }
                break;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.controllableModule.EvtOnMove -= Move;
        this.controllableModule.EvtOnCancelMove -= CancelMove;
        this.controllableModule.EvtOnJump -= Jump;
        this.controllableModule.EvtOnCancelJump -= CancelJump;
        this.controllableModule.EvtOnTurnUp -= TurnUp;
        this.controllableModule.EvtOnCancelTurnUp -= CancelTurnUp;
        this.collisionModule.EvtTriggerEnter2D -= CollisionModuleOnEvtTriggerEnter2D;
        this.collisionModule.EvtTriggerExit2D -= CollisionModuleOnEvtTriggerExit2D;
        this.fillBarManager.OnBarCompleteEvent -= Unstuck;
    }

    private void CollisionModuleOnEvtTriggerEnter2D(Collider2D obj)
    {
        string objTag = obj.tag;
        switch (objTag)
        {
            case "Ground":
                if (objTag.Equals("Ground") && obj.IsTouching(this.jumpResetCollider2D))
                {
                    ToggleStuckState(false);
                    ToggleGroundState(true);
                }

                if (CheckStuck(obj))
                {
                    ToggleStuckState(true);
                    ToggleGroundState(false);
                }
                
                break;
        }
    }

    private bool CheckStuck(Collider2D obj)
    {
        bool isStuck = !obj.IsTouching(jumpResetCollider2D) && 
                       ((obj.IsTouching(this.rightCollider2DBottom) && obj.IsTouching(this.rightCollider2DTop)) || 
                        (obj.IsTouching(leftCollider2DBottom) && obj.IsTouching(leftCollider2DTop)) || (obj.IsTouching(this.headLeftColider2D) && obj.IsTouching(this.headRightColider2D)));
        return isStuck;
    }

    private void ToggleGroundState(bool groundedState)
    {
        this.grounded = groundedState;
    }

    private void ToggleStuckState(bool stuckState)
    {
        this.stuck = stuckState;
        if (stuck)
        {
            this.fillBarManager.ToggleCanInteract(true);
        }
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

        if (grounded)
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
        if (!grounded) return;
        
        this.grounded = false;
        this.airDirection = new Vector3(this.horizontalAxis, 0, 0);
        this.rb.AddForce(Vector2.up * this.jumpForce);
        //this.rb.AddForceAtPosition(this.airDirection * airRotationForce, this.rb.position);
    }

    private void ProcessGroundMovement()
    {
        if (!this.moving || this.horizontalAxis == 0) return;

        Vector3 direction = new Vector3(this.horizontalAxis, 0, 0);
        Vector2 velocity = (direction * this.acceleration * Time.deltaTime);
        Vector2 finalVelocity = this.rb.velocity + velocity;
        finalVelocity.x = Mathf.Clamp(finalVelocity.x, -this.maxGroundedHorizontalVelocity, this.maxGroundedHorizontalVelocity);
        this.rb.velocity = finalVelocity;
    }

    private void ProcessAirMovement()
    {
        Vector3 actualVelocity = this.rb.velocity;
        this.horizontalAxis = this.controllableModule.HorizontalAxis;
        Vector3 direction = new Vector3(this.horizontalAxis, 0, 0);
        this.rb.AddForceAtPosition(airMovementRotationForce * direction, this.airRotationPivot.position);

        float clampedHorizontalVelocity = this.rb.velocity.x;
        clampedHorizontalVelocity = Mathf.Clamp(clampedHorizontalVelocity, -maxAirHorizontalVelocity, maxAirHorizontalVelocity);
        
        Vector3 clampedVelocity = actualVelocity;
        clampedVelocity.x = clampedHorizontalVelocity;
        this.rb.velocity = clampedVelocity;
    }

    private void FixedUpdate()
    {
        if (stuck) return;
        if (this.grounded) ProcessGroundMovement();
        else ProcessAirMovement();
    }
}
