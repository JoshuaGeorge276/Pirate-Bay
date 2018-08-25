using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadHandler : IDeviceHandler
{
    private int PadId;
    public GamePadBindings _bindings;
    private GamePadDevice device;

    public GamepadHandler(int a_padID)
    {
        _bindings = InputManager.Instance.GamePadDefaults();
        PadId = a_padID;
        device = new GamePadDevice(_bindings, PadId);
    }

    public GamepadHandler(int a_padID, GamePadBindings a_bindings)
    {
        _bindings = a_bindings;
        PadId = a_padID;
        device = new GamePadDevice(_bindings, PadId);
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
        return device.GetButton(a_value) == (int) InputButtonState.Down;
    }

    public bool GetButtonPressed(InputButtonValue a_value)
    {
        return device.GetButton(a_value) == (int) InputButtonState.Pressed;
    }

    public bool GetButtonUp(InputButtonValue a_value)
    {
        return device.GetButton(a_value) == (int) InputButtonState.Up;
    }
}
