using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance as T;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && !instance.Equals(this))
        {
            Destroy(this);
        }
        else
        {
            instance = this as T;
        }
    }

    public static S InstanceAs<S>() where S : BaseSingleton<S>
    {
        return instance as S;
    }
}
