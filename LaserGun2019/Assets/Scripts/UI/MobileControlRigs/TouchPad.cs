using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{    
    [SerializeField]
    private enum AxisOption
    {
        Both,
        OnlyHorizontal,
        OnlyVertical
    }
    [SerializeField] private AxisOption axesToUse = AxisOption.Both;

    [SerializeField] 
    private enum ControlStyle
    {
        Absolule,
        Relative,
        Swipe
    }
    [SerializeField] private ControlStyle controlStyle = ControlStyle.Absolule;

    [SerializeField] private string horizontalAxisName;
    [SerializeField] private string verticalAxisName;


    private bool useX;
    private bool useY;
    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;
    private Image image;
    private Vector3 pivotPoint;
    private bool dragging;
    private int fingerId = -1;    



    private void OnEnable()
    {
        RegisterVirtualAxes();
    }

    private void RegisterVirtualAxes()
    {
        useX = ((axesToUse == AxisOption.Both) || (axesToUse == AxisOption.OnlyHorizontal));
        useY = ((axesToUse == AxisOption.Both) || (axesToUse == AxisOption.OnlyVertical));

        if (useX)
        {
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
        }
        if (useY)
        {
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);
        }
    }

    private void Start()
    {
        image = GetComponent<Image>();
        pivotPoint = image.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        fingerId = eventData.pointerId;

        if (controlStyle != ControlStyle.Absolule)
        {
            pivotPoint = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        fingerId = -1;

        UpdateVirtualAxes(Vector3.zero);
    }

    private void Update()
    {
        if (!dragging)
        {
            return;
        }

        if (Input.touchCount >= fingerId + 1 && fingerId != -1)
        {
            Vector2 touchInput = Input.touches[fingerId].position;
            Vector2 pointerDelta = new Vector2(touchInput.x - pivotPoint.x, touchInput.y - pivotPoint.y);

            if (controlStyle == ControlStyle.Swipe)
            {                
                pivotPoint = touchInput;
            }

            UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
        }
    }

    private void UpdateVirtualAxes(Vector3 value)
    {
        if (useX)
        {
            CrossPlatformInputManager.SetAxis(horizontalAxisName, value.x);
        }
        if (useY)
        {
            CrossPlatformInputManager.SetAxis(verticalAxisName, value.y);
        }
    }

    private void OnDisable()
    {
        if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
        }
        if (CrossPlatformInputManager.AxisExists(verticalAxisName))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
        }
    }
}
