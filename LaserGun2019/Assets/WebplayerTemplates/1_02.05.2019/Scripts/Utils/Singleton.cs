using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T>: MonoBehaviour where T:Singleton<T>
{
    public static T Instance
    {
        get { return instance; }
    }

    public static bool IsInitialized
    {
        get{ return instance != null; }
    }

    private static T instance;



    public virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[SingleTon] Trying to instantiate a second instance of a singleton class.");
        }
        else
        {
            instance = (T)this;
        }
    }

    public virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }        
    }
}
