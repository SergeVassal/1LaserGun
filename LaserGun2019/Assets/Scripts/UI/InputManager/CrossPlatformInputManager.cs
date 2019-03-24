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

    private static VirtualInput activeInput;
    private static VirtualInput mobileInput;
    private static VirtualInput standaloneInput;



    static CrossPlatformInputManager()
    {
        mobileInput = new MobileInput();
        standaloneInput = new StandaloneInput();
#if MOBILE_INPUT
        activeInput = mobileInput;
#else
        activeInput=standaloneInput;
#endif
    }

    public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
    {
        switch (activeInputMethod)
        {

        }
    }



    public static bool AxisExists(string name)
    {
        return activeInput.AxisExists(name);
    }

    public static bool ButtonExists(string name)
    {
        return activeInput.ButtonExists(name);
    }

    public static void RegisterVirtualAxis(VirtualAxis axis)
    {
        if (activeInput.AxisExists(axis.Name))
        {
            Debug.LogError("There is already a virtual axis named " + axis.Name + " registered.");
        }
        else
        {
            activeInput.RegisterVirtualAxis(axis);
        }
    }

    public static void RegisterVirtualButton(VirtualButton button)
    {
        if (activeInput.ButtonExists(button.Name))
        {
            Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
        }
        else
        {
            activeInput.RegisterVirtualButton(button);
        }
    }

    public static void UnRegisterVirtualAxis(string name)
    {
        if (activeInput.AxisExists(name))
        {
            activeInput.UnRegisterVirtualAxis(name);
        }
    }

    public static void UnRegisterVirtualButton(string name)
    {
        if (activeInput.ButtonExists(name))
        {
            activeInput.UnRegisterVirtualButton(name);
        }
    }


    public abstract void SetAxis(string name, float value);

    public abstract void SetAxisPositive(string name);

    public abstract void SetAxisNegative(string name);

    public abstract void SetAxisZero(string name);

    public abstract float GetAxis(string name, bool raw);


    public abstract void SetButtonDown(string name);

    public abstract void SetButtonUp(string name);

    public abstract bool GetButtonDown(string name);

    public abstract bool GetButtonUp(string name);

    public abstract bool GetButton(string name);




    public class VirtualAxis
    {
        public string Name { get; private set; }

        private float value;


        public VirtualAxis(string name)
        {
            this.Name = name;
        }

        public void Update(float value)
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
        private float lastPressedFrame=-5f;
        private float releasedFrame=-5f;


        public VirtualButton(string name)
        {
            this.Name = name;
        }

        public void Press()
        {
            if (pressed)
            {
                return;
            }
            pressed = true;
            lastPressedFrame = Time.frameCount;
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
            return lastPressedFrame-Time.frameCount==-1f;
        }

        public bool GetButtonUp()
        {
            return (releasedFrame==Time.frameCount-1f);
        }
    }
	
}
