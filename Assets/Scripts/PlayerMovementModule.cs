using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementModule : MovementModule
{
    [Header("Controllable module")]
    [SerializeField]protected PlatformControllableModule controllableModule;
    protected float horizontalAxis;
    public float HorizontalAxis => this.horizontalAxis;

    protected override void Awake()
    {
        base.Awake();
        controllableModule.EvtOnMove += Move;
    }

    protected override void OnStateChange()
    {
        base.OnStateChange();
    }

    protected override void OnStateProcess()
    {
        base.OnStateProcess();
    }

    protected override void Move()
    {
        base.Move();
        this.character.State = CHARACTER_STATE.MOVE;

        if (this.character.State != CHARACTER_STATE.MOVE) return;
        
        this.moving = true;
        this.horizontalAxis = this.controllableModule.HorizontalAxis;
    }

    protected override void Jump()
    {
        base.Jump();
    }

    protected void Update()
    {
        if (!this.moving || this.horizontalAxis == 0) return;

        Vector3 direction = new Vector3(this.horizontalAxis * Time.deltaTime, 0, 0);
        this.character.gameObject.transform.position = this.character.gameObject.transform.position + direction;
    }

    protected void FixedUpdate()
    {
        
    }
}
