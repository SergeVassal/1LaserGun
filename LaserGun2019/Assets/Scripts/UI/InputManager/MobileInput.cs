﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : VirtualInput 
{
    public override void SetAxis(string name, float value)
    {
        if (!AxisExists(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].UpdateAxis(value);
    }
    public override void SetAxisPositive(string name)
    {
        if (!AxisExists(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].UpdateAxis(1f);
    }
    public override void SetAxisNegative(string name)
    {
        if (!AxisExists(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].UpdateAxis(-1f);
    }
    public override void SetAxisZero(string name)
    {
        if (!AxisExists(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].UpdateAxis(0f);
    }
    public override float GetAxis(string name, bool raw)
    {
        if (!AxisExists(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        return virtualAxes[name].GetValue();
    }


    public override void SetButtonDown(string name)
    {
        if (!ButtonExists(name))
        {
            throw new Exception("There's no such button registered!");
        }
        virtualButtons[name].Press();
    }
    public override void SetButtonUp(string name)
    {
        if (!ButtonExists(name))
        {
            throw new Exception("There's no such button registered!");
        }
        virtualButtons[name].Release();
    }
    public override bool GetButton(string name)
    {
        if (!ButtonExists(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButton();
    }
    public override bool GetButtonDown(string name)
    {
        if (!ButtonExists(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButtonDown();

    }
    public override bool GetButtonUp(string name)
    {
        if (!ButtonExists(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButtonUp();

    }
}
