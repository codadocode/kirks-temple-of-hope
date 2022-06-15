using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : SingletonBase<LevelBase>
{
    public Action OnLevelStart = null;
    public Action OnLevelEnd = null;
    public Action OnAfterLevelSetup = null;
    public Action OnBeforeLevelSetup = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        OnBeforeLevelSetup?.Invoke();
        LevelSetup();
        OnAfterLevelSetup?.Invoke();
        //StartLevel();
    }

    protected virtual void LevelSetup()
    {
        //SETUP LEVEL
    }

    protected virtual void StartLevel()
    {
        //START LEVEL
        OnLevelStart?.Invoke();
    }
}
