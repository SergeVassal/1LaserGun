using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualInput
{
    protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> virtualAxes =
        new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

    protected Dictionary<string, CrossPlatformInputManager.VirtualButton> virtualButtons =
        new Dictionary<string, CrossPlatformInputManager.VirtualButton>();



    public bool AxisExists(string name)
    {
        return virtualAxes.ContainsKey(name);
    }

    public bool ButtonExists(string name)
    {
        return virtualButtons.ContainsKey(name);
    }

    public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
    {
        if (virtualAxes.ContainsKey(axis.Name))
        {
            Debug.LogError("There is already a virtual axis named " + axis.Name + " registered.");
        }
        else
        {
            virtualAxes.Add(axis.Name, axis);
        }
    }

    public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
    {
        if (virtualButtons.ContainsKey(button.Name))
        {
            Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
        }
        else
        {
            virtualButtons.Add(button.Name, button);
        }
    }

    public void UnRegisterVirtualAxis(string name)
    {
        if (virtualAxes.ContainsKey(name))
        {
            virtualAxes.Remove(name);
        }
    }

    public void UnRegisterVirtualButton(string name)
    {
        if (virtualAxes.ContainsKey(name))
        {
            virtualButtons.Remove(name);
        }
    }


    public abstract void SetAxis(string name,float value);

    public abstract void SetAxisPositive(string name);

    public abstract void SetAxisNegative(string name);

    public abstract void SetAxisZero(string name);

    public abstract float GetAxis(string name,bool raw);


    public abstract void SetButtonDown(string name);

    public abstract void SetButtonUp(string name);

    public abstract bool GetButtonDown(string name);

    public abstract bool GetButtonUp(string name);

    public abstract bool GetButton(string name);  
}
