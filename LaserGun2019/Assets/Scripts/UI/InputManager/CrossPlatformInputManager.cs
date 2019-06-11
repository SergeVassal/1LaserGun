using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossPlatformInputManager 
{
    public enum ActiveInputMethod
    {
        Standalone,
        Mobile
    }
    
    private static VirtualInput mobileInput;
    private static VirtualInput standaloneInput;
    private static VirtualInput activeInput;



    static CrossPlatformInputManager()
    {
        standaloneInput = new StandaloneInput();
        mobileInput = new MobileInput();
#if !MOBILE_INPUT
        activeInput=standaloneInput;
#elif MOBILE_INPUT
        activeInput = mobileInput;
#endif
    }

    public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
    {
        switch (activeInputMethod)
        {
            case ActiveInputMethod.Standalone:
                activeInput = standaloneInput;
                break;
            case ActiveInputMethod.Mobile:
                activeInput = mobileInput;
                break;
        }
    }

    public static bool AxisExists(string name)
    {
        return activeInput.AxisExists(name);
    }

    public static void RegisterVirtualAxis(VirtualAxis axis)
    {
        activeInput.RegisterVirtualAxis(axis);
    }

    public static void UnRegisterVirtualAxis(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        activeInput.UnRegisterVirtualAxis(name);
    }


    public static bool ButtonExists(string name)
    {
        return activeInput.ButtonExists(name);
    }

    public static void RegisterVirtualButton(VirtualButton button)
    {
        activeInput.RegisterVirtualButton(button);
    }

    public static void UnRegisterVirtualButton(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        activeInput.UnRegisterVirtualButton(name);
    }


    public static void SetAxis(string name, float value)
    {
        activeInput.SetAxis(name, value);
    }
    public static void SetAxisPositive(string name)
    {
        activeInput.SetAxisPositive(name);
    }
    public static void SetAxisNegative(string name)
    {
        activeInput.SetAxisNegative(name);
    }
    public static void SetAxisZero(string name)
    {
        activeInput.SetAxisZero(name);
    }
    public static float GetAxis(string name)
    {
        return activeInput.GetAxis(name, false);
    }
    public static float GetAxisRaw(string name)
    {
        return activeInput.GetAxis(name, true);
    }
    private static float GetAxis(string name,bool raw)
    {
        return activeInput.GetAxis(name, raw);
    }
                  

    public static void SetButtonDown(string name)
    {
        activeInput.SetButtonDown(name);
    }
    public static void SetButtonUp(string name)
    {
        activeInput.SetButtonUp(name);
    }
    public static bool GetButton(string name)
    {
        return activeInput.GetButton(name);
    }
    public static bool GetButtonDown(string name)
    {
        return activeInput.GetButtonDown(name);
    }
    public static bool GetButtonUp(string name)
    {
        return activeInput.GetButtonUp(name);
    }



    public class VirtualAxis
    {
        public string Name { get; private set; }

        private float value;


        public VirtualAxis(string name)
        {
            Name = name;
        }

        public void UpdateAxis(float value)
        {
            this.value = value;
        }

        public float GetValue()
        {
            return value;
        }
    }

    public class VirtualButton
    {
        public string Name { get; private set; }

        private bool pressed;
        private float lastpressedFrame=-5f;
        private float releasedFrame=-5f;


        public VirtualButton(string name)
        {
            Name = name;
        }

        public void Press()
        {
            if (!pressed)
            {
                pressed = true;
                lastpressedFrame = Time.frameCount;
            }
        }

        public void Release()
        {
            pressed = false;
            releasedFrame = Time.frameCount;

        }

        public bool GetButton()
        {
            return pressed;
        }

        public bool GetButtonDown()
        {
            return (Time.frameCount == lastpressedFrame + 1f);  
        }

        public bool GetButtonUp()
        {
            return (Time.frameCount == releasedFrame + 1);            
        }
    }



}
