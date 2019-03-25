using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    [SerializeField] private bool lockCursor = true;

    private bool previouslyLocked;



    public void SetCursorLock(bool value)
    {
        lockCursor = value;
    }

    private void FixedUpdate()
    {
        if (lockCursor)
        {
            CursorLockUpdate();
        }        
        if (previouslyLocked && !lockCursor)
        {
            DisableCursorLock();
        }
        previouslyLocked = lockCursor;
    }

    private void CursorLockUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisableCursorLock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
