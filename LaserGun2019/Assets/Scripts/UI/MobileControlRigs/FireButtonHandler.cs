using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    CrossPlatformInputManager.VirtualButton fireButton;



    private void OnEnable()
    {
        CreateVirtualButton();
    }


    private void CreateVirtualButton()
    {
        fireButton = new CrossPlatformInputManager.VirtualButton("Fire1");
        CrossPlatformInputManager.RegisterVirtualButton(fireButton);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        fireButton.Press();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        fireButton.Release();
    }
}
