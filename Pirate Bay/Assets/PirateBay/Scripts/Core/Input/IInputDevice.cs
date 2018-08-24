using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IInputDevice
{
    void UpdateDevice();
    void LateUpdateDevice();
    void ResetDevice();

    float GetAxis(InputAxisValue a_value);
    int GetButton(InputButtonValue a_value);
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