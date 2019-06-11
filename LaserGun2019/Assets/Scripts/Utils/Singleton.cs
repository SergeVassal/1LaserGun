using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T>: MonoBehaviour where T:Singleton<T>
{
    public static T Instance { get; private set; }
    public static bool IsInitialized { get { return Instance != null; } }


    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            throw new Exception("You're trying to instantiate a second copy of a singleton class!");
        }
    }

    protected virtual void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
