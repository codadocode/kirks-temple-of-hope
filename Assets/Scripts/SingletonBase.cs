using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    private static T instance; 
    
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            // INSTANCE ALREADY EXISTS ERROR
        }
        else
        {
            instance = (T)this;
        }
    }
}
