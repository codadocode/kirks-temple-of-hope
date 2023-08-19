using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour
{
    protected CustomInput customInput;

    protected virtual void Awake()
    {
        this.customInput = new CustomInput();
        EnableCustomInput();
    }

    protected virtual void OnDestroy()
    {
        DisableCustomInput();
    }

    protected virtual void OnDisable()
    {
        DisableCustomInput();
    }

    protected virtual void OnEnable()
    {
        EnableCustomInput();
    }

    private void DisableCustomInput()
    {
        this.customInput.Disable();
    }

    private void EnableCustomInput()
    {
        this.customInput.Enable();
    }
}
