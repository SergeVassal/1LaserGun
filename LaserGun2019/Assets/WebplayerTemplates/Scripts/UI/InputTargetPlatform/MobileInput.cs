﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : VirtualInput
{   
    public override void SetButtonDown(string name)
    {
        if (!virtualButtons.ContainsKey(name))
        {
            throw new Exception("There's no such button registered!");
        }
        virtualButtons[name].Press();
    }

    public override void SetButtonUp(string name)
    {
        if (!virtualButtons.ContainsKey(name))
        {
            throw new Exception("There's no such button registered!");
        }
        virtualButtons[name].Release();
    }

    public override bool GetButton(string name)
    {
        if (!virtualButtons.ContainsKey(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButton();
    }

    public override bool GetButtonDown(string name)
    {
        if (!virtualButtons.ContainsKey(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButtonDown();
    }

    public override bool GetButtonUp(string name)
    {
        if (!virtualButtons.ContainsKey(name))
        {
            throw new Exception("There's no such button registered!");
        }
        return virtualButtons[name].GetButtonUp();
    }


    public override void SetAxis(string name, float value)
    {
        if (!virtualAxes.ContainsKey(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].Update(value);
    }

    public override void SetAxisPositive(string name)
    {
        if (!virtualAxes.ContainsKey(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].Update(1f);
    }

    public override void SetAxisNegative(string name)
    {
        if (!virtualAxes.ContainsKey(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].Update(-1f);
    }

    public override void SetAxisZero(string name)
    {
        if (!virtualAxes.ContainsKey(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        virtualAxes[name].Update(0f);
    }

    public override float GetAxis(string name, bool raw)
    {
        if (!virtualAxes.ContainsKey(name))
        {
            throw new Exception("There's no such axis registered!");
        }
        return virtualAxes[name].GetValue;
    }

}
