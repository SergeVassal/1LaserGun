using System;
using UnityEngine;

[Serializable]
public class RotationController
{
    [SerializeField] private float standaloneSensitivityX = 1f;
    [SerializeField] private float standaloneSensitivityY = 1f;

    [SerializeField] private float MinimumX = -90F;
    [SerializeField] private float MaximumX = 90F;

    [SerializeField] private bool clampVerticalRotation = true;
    [SerializeField] private bool standaloneSmooth;
    [SerializeField] private float standaloneSmoothTime = 0.5f;

    [SerializeField] private bool lockCursor = true;

    [SerializeField] private float mobileInputValueToDegreesRatio = 180f;
    [SerializeField] private float mobileInputAreaForRotationX = Screen.width / 2;
    [SerializeField] private float mobileInputAreaForRotationY = Screen.height / 2;


    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;

    private bool cursorIsLocked = true;


    
    public void Init(Transform character, Transform camera)
    {
#if MOBILE_INPUT
        standaloneSmooth = false;
#endif
        characterTargetRot = character.localRotation;
        cameraTargetRot = camera.localRotation;
    }


    public void ApplyRotation(Transform character, Transform camera)
    {        
        float inputAxisX = CrossPlatformInputManager.GetAxis("Mouse X");
        float inputAxisY = CrossPlatformInputManager.GetAxis("Mouse Y");

#if MOBILE_INPUT
        float inputRelativeToDegreesX = inputAxisX / mobileInputAreaForRotationX * mobileInputValueToDegreesRatio;
        float inputRelativeToDegreesY = inputAxisY / mobileInputAreaForRotationY * mobileInputValueToDegreesRatio;

        characterTargetRot *= Quaternion.Euler(0f, inputRelativeToDegreesX, 0f);
        cameraTargetRot *= Quaternion.Euler(-inputRelativeToDegreesY, 0f, 0f);
#endif

#if !MOBILE_INPUT
        float yRot = inputAxisX * standaloneSensitivityX;
        float xRot = inputAxisY * standaloneSensitivityY;

        characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
#endif

        if (clampVerticalRotation)
            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        if (standaloneSmooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRot,
                standaloneSmoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRot,
                standaloneSmoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = characterTargetRot;
            camera.localRotation = cameraTargetRot;
        }
    }


    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }


    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }


    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
