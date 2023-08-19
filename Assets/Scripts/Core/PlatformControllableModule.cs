using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformControllableModule : InputModule
{
    protected float horizontalAxis;
    public event Action EvtOnMove;
    public event Action EvtOnJump;

    public float HorizontalAxis
    {
        get
        {
            return this.horizontalAxis;
        }
    }

    protected virtual void Start()
    {
        this.customInput.gameplay.move.performed += MoveOnPerformed;
        this.customInput.gameplay.jump.performed += JumpOnPerformed;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.customInput.gameplay.move.performed -= MoveOnPerformed;
        this.customInput.gameplay.jump.performed -= JumpOnPerformed;
    }

    protected virtual void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        OnJump();
    }

    protected virtual void MoveOnPerformed(InputAction.CallbackContext obj)
    {
        this.horizontalAxis = obj.ReadValue<float>();
        OnMove();
    }

    protected virtual void OnMove()
    {
        this.EvtOnMove?.Invoke();
    }

    protected virtual void OnJump()
    {
        this.EvtOnJump?.Invoke();
    }
}
