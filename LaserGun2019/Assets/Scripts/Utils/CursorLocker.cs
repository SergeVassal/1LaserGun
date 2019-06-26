using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour 
{
    [SerializeField] private bool lockCursor = true;



    public void SetCursorLock(bool lockCursor)
    {
        this.lockCursor = lockCursor;
        UpdateCursorLock();
    }

    private void Start()
    {
        UpdateCursorLock();
    }

    private void UpdateCursorLock()
    {
        if (lockCursor)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
