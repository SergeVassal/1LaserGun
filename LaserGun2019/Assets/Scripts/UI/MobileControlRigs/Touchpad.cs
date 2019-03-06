using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Touchpad : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private enum AxisOption
    {
        Both,
        OnlyHorizontal,
        OnlyVertical
    }
    [SerializeField] private AxisOption axesToUse = AxisOption.Both;

    [SerializeField] private enum ControlStyle
    {
        Absolute,
        Relative,
        Swipe
    }
    [SerializeField] private ControlStyle controlStyle = ControlStyle.Absolute;

    [SerializeField] private string horizontalAxisName;
    [SerializeField] private string verticalAxisName;

    private bool useX;
    private bool useY;

    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;

    private bool dragging;
    private int fingerID = -1;

    private Vector2 previousTouchPos;

    private Vector3 center;
    private Image image;



    private void OnEnable()
    {
        CreateVirtualAxis();
    }


    private void Start()
    {
        image = GetComponent<Image>();
        center = image.transform.position;
    }


    private void CreateVirtualAxis()
    {
        useX = (axesToUse == AxisOption.Both) || (axesToUse == AxisOption.OnlyHorizontal);
        useY = (axesToUse == AxisOption.Both) || (axesToUse == AxisOption.OnlyVertical);

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


    public void OnPointerDown(PointerEventData data)
    {
        dragging = true;
        fingerID = data.pointerId;

        if (controlStyle != ControlStyle.Absolute)
        {
            center = data.position;
        }
    }


    public void OnPointerUp(PointerEventData data)
    {
        dragging = false;
        fingerID = -1;

        UpdateVirtualAxes(Vector3.zero);
    }


    private void UpdateVirtualAxes(Vector3 value)
    {
        if (useX)
        {
            horizontalVirtualAxis.Update(value.x);
        }

        if (useY)
        {
            verticalVirtualAxis.Update(value.y);
        }
    }


    private void Update()
    {
        if (!dragging)
        {
            return;
        }

        if (Input.touchCount >= fingerID + 1 && fingerID != -1)
        {
            Vector2 touchInput = Input.touches[fingerID].position;
            Vector2 pointerDelta = new Vector2(touchInput.x - center.x, touchInput.y - center.y);

            if (controlStyle == ControlStyle.Swipe)
            {
                previousTouchPos = touchInput;
                center = previousTouchPos;
            }

            UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
        }
    }


    void OnDisable()
    {
        if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
            CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

        if (CrossPlatformInputManager.AxisExists(verticalAxisName))
            CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
    }


}
