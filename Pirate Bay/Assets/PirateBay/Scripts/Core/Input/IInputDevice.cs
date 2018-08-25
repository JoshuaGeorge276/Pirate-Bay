using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IInputDevice
{
    void SetupDevice(Bindings a_bindings);
    void UpdateDevice();
    void LateUpdateDevice();
    void ResetDevice();

    float GetAxis(InputAxisValue a_value);
    int GetButton(InputButtonValue a_value);
}

public static class IInputDeviceHelper
{
    public static void UpdateButtons(this IInputDevice a_iDevice, int a_buttonCount,
                                        Dictionary<InputButtonValue, KeyCode> a_keyLookup,
                                        ref sbyte[] a_values)
    {
        for (int i = 0; i < a_buttonCount; ++i)
        {
            sbyte currentValue = a_values[i];
            KeyCode key;

            if(!a_keyLookup.TryGetValue((InputButtonValue) i, out key))
            {
                continue;
            }

            switch (currentValue)
            {
                case (sbyte) InputButtonState.None:

                    if (Input.GetKeyDown(key))
                        a_values[i] = (sbyte) InputButtonState.Down;

                    continue;
                case (sbyte) InputButtonState.Down:

                    if (Input.GetKey(key))
                    {
                        a_values[i] = (sbyte) InputButtonState.Pressed;
                    }
                    else if (Input.GetKeyUp(key))
                    {
                        a_values[i] = (sbyte) InputButtonState.Up;
                    }

                    continue;
                case (sbyte) InputButtonState.Pressed:

                    if (Input.GetKeyUp(key))
                        a_values[i] = (sbyte) InputButtonState.Up;

                    continue;
                case (sbyte) InputButtonState.Up:
                     
                    if (Input.GetKeyDown(key))
                        a_values[i] = (sbyte) InputButtonState.Down;

                    a_values[i] = (sbyte) InputButtonState.None;
                    continue;
            }
        }
    }
}

public enum InputAxisValue
{
    LeftX,
    LeftY,
    RightX,
    RightY,
    COUNT
}

public enum InputButtonValue
{
    LeftStickButton,
    RightStickButton,
    DPadUp,
    DPadDown,
    DPadLeft,
    DPadRight,
    Action1,
    Action2,
    Action3,
    Action4,
    LeftTrigger,
    RightTrigger,
    LeftBumper,
    RightBumper,
    Start,
    Return,
    Select,
    Pause,
    Menu,
    Options,
    COUNT
}

public enum InputButtonState
{
    None,
    Down,
    Pressed,
    Up,
    COUNT
}