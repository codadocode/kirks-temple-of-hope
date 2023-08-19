using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected CHARACTER_STATE state;
    public event Action EvtChangeState;
    public event Action EvtProcessState;

    public CHARACTER_STATE State
    {
        get
        {
            return this.state;
        }

        set
        {
            this.state = value;
            ChangeState();
        }
    }

    protected virtual void ChangeState()
    {
        this.EvtChangeState?.Invoke();
        ProcessState();
    }

    protected virtual void ProcessState()
    {
        this.EvtProcessState?.Invoke();
    }
}
