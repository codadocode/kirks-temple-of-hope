using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlataformControllableModule : PlatformControllableModule
{
    public event Action EvtOnTurnUp;
    public event Action EvtOnCancelTurnUp;
    
    protected override void Start()
    {
        base.Start();
        this.customInput.gameplay.turnup.performed += TurnUpPerformed;
        this.customInput.gameplay.turnup.performed += TurnUpCanceled;
    }

    private void TurnUpCanceled(InputAction.CallbackContext obj)
    {
        OnCancelTurnUp();
    }

    private void TurnUpPerformed(InputAction.CallbackContext obj)
    {
        OnTurnUp();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.customInput.gameplay.turnup.performed -= TurnUpPerformed;
        this.customInput.gameplay.turnup.performed -= TurnUpCanceled;
    }

    private void OnTurnUp()
    {
        this.EvtOnTurnUp?.Invoke();
    }

    private void OnCancelTurnUp()
    {
        this.EvtOnCancelTurnUp?.Invoke();
    }
}
