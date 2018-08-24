using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KeyboardHandler : IDeviceHandler
{

    public Bindings _bindings = Presets.GetKeyboardBindings();
    private KeyboardDevice keyboardDevice;

    public KeyboardHandler(Bindings? a_bindings = null)
    {
        if (a_bindings.HasValue)
        {
            _bindings = a_bindings.GetValueOrDefault();
        }

        keyboardDevice = new KeyboardDevice(_bindings);
    }

    public void ResetDevice()
    {
        keyboardDevice.ResetDevice();
    }

    public void UpdateDevice()
    {
        keyboardDevice.UpdateDevice();
    }

    public void LateUpdateDevice()
    {

    }

    public float GetAxis(InputAxisValue a_value)
    {
        return keyboardDevice.GetAxis(a_value);
    }

    public bool GetButtonDown(InputButtonValue a_value)
    {
        return keyboardDevice.GetButton(a_value) == (int)InputButtonState.Down;
    }

    public bool GetButtonPressed(InputButtonValue a_value)
    {
        return keyboardDevice.GetButton(a_value) == (int) InputButtonState.Pressed;
    }

    public bool GetButtonUp(InputButtonValue a_value)
    {
        return keyboardDevice.GetButton(a_value) == (int) InputButtonState.Up;
    }
}
