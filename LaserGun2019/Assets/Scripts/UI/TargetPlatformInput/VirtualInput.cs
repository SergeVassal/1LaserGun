using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualInput
{
    public Dictionary<string, CrossPlatformInputManager.VirtualAxis> virtualAxis = 
        new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

    public Dictionary<string, CrossPlatformInputManager.VirtualButton> virtualButtons =
        new Dictionary<string, CrossPlatformInputManager.VirtualButton>();



}
