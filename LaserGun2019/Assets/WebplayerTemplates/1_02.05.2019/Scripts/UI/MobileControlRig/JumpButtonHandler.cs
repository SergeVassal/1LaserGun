using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    CrossPlatformInputManager.VirtualButton jumpButton;



    private void OnEnable()
    {
        CreateVirtualButton();
    }

    private void CreateVirtualButton()
    {
        jumpButton = new CrossPlatformInputManager.VirtualButton("Jump");
        CrossPlatformInputManager.RegisterVirtualButton(jumpButton);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        jumpButton.Press();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        jumpButton.Release();
    }
}
