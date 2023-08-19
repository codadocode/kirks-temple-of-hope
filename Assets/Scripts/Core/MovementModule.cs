using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : BaseModule
{
    [Header("Character configuration")]
    [SerializeField] protected BaseCharacter character;
    protected bool moving = false;

    protected virtual void Awake()
    {
        this.character.EvtChangeState += OnStateChange;
        this.character.EvtProcessState += OnStateProcess;
    }

    protected virtual void OnDestroy()
    {
        this.character.EvtChangeState -= OnStateChange;
        this.character.EvtProcessState -= OnStateProcess;
    }

    protected override void OnStateChange()
    {
        base.OnStateChange();
    }

    protected override void OnStateProcess()
    {
        base.OnStateProcess();
    }

    protected virtual void Move()
    {
        
    }

    protected virtual void Jump()
    {
        
    }
}
