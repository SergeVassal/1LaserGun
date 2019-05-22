using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T>: MonoBehaviour where T:Singleton<T>
{
    public static T Instance { get; private set; }
    public static bool IsInitialized { get{ return Instance != null; }  }



    protected virtual void Awake()
    {
        if (Instance != null)
        {
            throw new System.Exception("You are trying to instantiate a second instance of a singleton class!");
        }
        else
        {
            Instance = (T)this;    
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

