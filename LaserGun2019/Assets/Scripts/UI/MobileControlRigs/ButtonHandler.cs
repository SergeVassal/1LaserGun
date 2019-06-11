using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string buttonName;

    private CrossPlatformInputManager.VirtualButton button;


    private void OnEnable()
    {
        button = new CrossPlatformInputManager.VirtualButton(buttonName);
        CrossPlatformInputManager.RegisterVirtualButton(button);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CrossPlatformInputManager.SetButtonDown(buttonName);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CrossPlatformInputManager.SetButtonUp(buttonName);
    }
}
