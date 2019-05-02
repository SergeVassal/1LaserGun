using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterRotationController 
{
    [SerializeField] private bool standaloneSmooth;
    [SerializeField] private float standaloneSmoothTime;
    [SerializeField] private float mobileWholeRotationTouchAreaToDegrees = 180f;
    [SerializeField] private float standaloneSensitivityX;
    [SerializeField] private float standaloneSensitivityY;
    [SerializeField] private bool clampVerticalRotation = true;
    [SerializeField] private float MinimumX = -90f;
    [SerializeField] private float MaximumX = 90f;    

    private Quaternion characterTargetRotation;
    private Quaternion cameraTargetRotation;
    private Transform characterBodyTransform;
    private Transform cameraTransform;
    private float mobileTouchAreaHorizontalSize;
    private float mobileTouchAreaVerticalSize;



    public void InitializeRotationController(Transform characterBody, Transform camera)
    {
#if MOBILE_INPUT
        standaloneSmooth=false;
#endif
        characterBodyTransform = characterBody;
        cameraTransform = camera;

        characterTargetRotation = characterBody.localRotation;
        cameraTargetRotation = camera.localRotation;

        mobileTouchAreaHorizontalSize = Screen.width / 2;
        mobileTouchAreaVerticalSize = Screen.height / 2;
    }

    public void RotateCharacterAndCamera()
    {
        Vector2 rotationInputRaw=GetRotationInput();

#if MOBILE_INPUT
        Vector2 mobileRotationDelta = GetRotDeltaRelativeToTouchAreaSizeAndSensitivity(rotationInputRaw);
        CalculateRotationQuaternion(mobileRotationDelta);
        
#endif

#if !MOBILE_INPUT
        Vector2 standaloneRotationDelta = GetStandaloneRotDeltaWithSensitivity(rotationInputRaw);
        CalculateRotationQuaternion(standaloneRotationDelta);

#endif
        ApplyRotation();            
    }

    private Vector2 GetRotationInput()
    {
        float horizontalInputraw = CrossPlatformInputManager.GetAxis("Mouse X");
        float verticalInputRaw = CrossPlatformInputManager.GetAxis("Mouse Y");

        return new Vector2(horizontalInputraw, verticalInputRaw);
    }

    private Vector2 GetRotDeltaRelativeToTouchAreaSizeAndSensitivity(Vector2 rotationInputRaw)
    {
        float rotDeltaX = rotationInputRaw.x / mobileTouchAreaHorizontalSize * mobileWholeRotationTouchAreaToDegrees;
        float rotDeltaY = rotationInputRaw.y / mobileTouchAreaVerticalSize * mobileWholeRotationTouchAreaToDegrees;

        return new Vector2(rotDeltaX, rotDeltaY);
    }

    private Vector2 GetStandaloneRotDeltaWithSensitivity(Vector2 rotationInputRaw)
    {
        float rotDeltaX = rotationInputRaw.x * standaloneSensitivityX;
        float rotDeltaY = rotationInputRaw.y * standaloneSensitivityY;

        return new Vector2(rotDeltaX, rotDeltaY);
    }

    private void CalculateRotationQuaternion(Vector2 rotationDelta)
    {
        characterTargetRotation *= Quaternion.Euler(0f, rotationDelta.x, 0f);
        cameraTargetRotation *= Quaternion.Euler(-rotationDelta.y, 0f, 0f);

        if (clampVerticalRotation)
            cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation);
    }

    private void ApplyRotation()
    {
        if (standaloneSmooth)
        {
            ApplyRotationWithStandaloneSmooth();
        }
        else
        {
            ApplyRotationWithoutStandaloneSmooth();
        }
    }

    private void ApplyRotationWithoutStandaloneSmooth()
    {
        characterBodyTransform.localRotation = characterTargetRotation;
        cameraTransform.localRotation = cameraTargetRotation;
    }

    private void ApplyRotationWithStandaloneSmooth()
    {
        characterBodyTransform.localRotation = Quaternion.Slerp(characterBodyTransform.localRotation, characterTargetRotation,
            standaloneSmoothTime * Time.deltaTime);
        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraTargetRotation,
            standaloneSmoothTime * Time.deltaTime);
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
}
