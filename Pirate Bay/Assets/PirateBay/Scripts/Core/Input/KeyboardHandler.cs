using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KeyboardHandler : IDeviceHandler
{

    public KeyboardBindings _bindings;
    private KeyboardDevice device;


    public KeyboardHandler()
    {
        _bindings = InputManager.Instance.KeyboardDefaults();
        device = new KeyboardDevice(_bindings);
    }

    public KeyboardHandler(KeyboardBindings a_bindings)
    {
        if (a_bindings)
            _bindings = a_bindings;

        device = new KeyboardDevice(_bindings);
    }

    public void ResetDevice()
    {
        device.ResetDevice();
    }

    public void UpdateDevice()
    {
        device.UpdateDevice();
    }

    public void LateUpdateDevice()
    {
    }

    public float GetAxis(InputAxisValue a_value)
    {
        return device.GetAxis(a_value);
    }

    public bool GetButtonDown(InputButtonValue a_value)
    {
        return device.GetButton(a_value) == (int)InputButtonState.Down;
    }

    public bool GetButtonPressed(InputButtonValue a_value)
    {
        return device.GetButton(a_value) == (int) InputButtonState.Pressed;
    }

    public bool GetButtonUp(InputButtonValue a_value)
    {
        return device.GetButton(a_value) == (int) InputButtonState.Up;
    }

    public float GetLastInputTime()
    {
        return device.LastInputTime();
    }
}
