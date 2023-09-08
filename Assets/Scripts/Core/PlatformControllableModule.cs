using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformControllableModule : InputModule
{
    protected float horizontalAxis;
    public event Action EvtOnMove;
    public event Action EvtOnCancelMove;
    public event Action EvtOnJump;
    public event Action EvtOnCancelJump;

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
        
        this.customInput.gameplay.move.canceled += MoveOnCanceled;
        this.customInput.gameplay.jump.canceled += JumpOnCanceled;
    }

    private void JumpOnCanceled(InputAction.CallbackContext obj)
    {
        OnCancelJump();
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj)
    {
        OnCancelMove();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.customInput.gameplay.move.performed -= MoveOnPerformed;
        this.customInput.gameplay.jump.performed -= JumpOnPerformed;
        this.customInput.gameplay.move.canceled -= MoveOnCanceled;
        this.customInput.gameplay.jump.canceled -= JumpOnCanceled;
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

    protected virtual void OnCancelMove()
    {
        this.horizontalAxis = 0;
        this.EvtOnCancelMove?.Invoke();
    }

    protected virtual void OnCancelJump()
    {
        this.EvtOnCancelJump?.Invoke();
    }
}
