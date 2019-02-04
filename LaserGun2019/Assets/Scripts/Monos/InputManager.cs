using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private AbstractGun gun;

    private void Start()
    {        
        if (GetComponentInChildren<AbstractGun>() != null)
        {
            gun = GetComponentInChildren<AbstractGun>();            
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gun.FireGun();
        }

    }

}
