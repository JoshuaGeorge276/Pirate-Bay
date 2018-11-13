using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class KeyboardDevice : IInputDevice
{
    private TwoAxisInput keyboardAxis;
    private TwoAxisInput mouseAxis;
    private OneAxisInput mouseWheelAxis;

    private sbyte[] buttonValues;
    private Dictionary<InputButtonValue, KeyCode> keyLookup;

    private KeyboardBindings currentBindings;

    private int buttonCount = (int) InputButtonValue.COUNT;


    private float lastInputTime = 0f;
    public float LastInputTime()
    {
        return lastInputTime;
    }

    public KeyboardDevice(KeyboardBindings _bindings)
    {
        buttonValues = new sbyte[buttonCount];
        keyLookup = new Dictionary<InputButtonValue, KeyCode>(buttonCount);

        currentBindings = _bindings ? _bindings : InputManager.Instance.KeyboardDefaults();

        SetupDevice(currentBindings);
    }

    public void ResetDevice()
    {
        keyboardAxis.ResetAxis();
        mouseAxis.ResetAxis();
        mouseWheelAxis.ResetAxis();
        ResetButtons();
    }

    public void SetupDevice(Bindings a_bindings)
    {
        KeyboardBindings bindings = (KeyboardBindings) a_bindings;

        currentBindings = bindings ? bindings : InputManager.Instance.KeyboardDefaults();

        keyboardAxis = new TwoAxisInput(currentBindings.LeftXAxis, currentBindings.LeftYAxis);
        mouseAxis = new TwoAxisInput(currentBindings.RightXAxis, currentBindings.RightYAxis);
        mouseWheelAxis  = new OneAxisInput(currentBindings.MouseWheelAxis);

        keyLookup[InputButtonValue.LeftStickButton]  = currentBindings.LeftStickButton;
        keyLookup[InputButtonValue.RightStickButton] = currentBindings.RightStickButton;

        keyLookup[InputButtonValue.DPadDown]         = currentBindings.DPadDown;
        keyLookup[InputButtonValue.DPadUp]           = currentBindings.DPadUp;
        keyLookup[InputButtonValue.DPadLeft]         = currentBindings.DPadLeft;
        keyLookup[InputButtonValue.DPadRight]        = currentBindings.DPadRight;

        
        keyLookup[InputButtonValue.Action1]          = currentBindings.Action1;
        keyLookup[InputButtonValue.Action2]          = currentBindings.Action2;
        keyLookup[InputButtonValue.Action3]          = currentBindings.Action3;
        keyLookup[InputButtonValue.Action4]          = currentBindings.Action4;


        keyLookup[InputButtonValue.LeftTrigger]      = currentBindings.LeftTrigger;
        keyLookup[InputButtonValue.RightTrigger]     = currentBindings.RightTrigger;

        keyLookup[InputButtonValue.LeftBumper]       = currentBindings.LeftBumper;
        keyLookup[InputButtonValue.RightBumper]      = currentBindings.RightBumper;

        keyLookup[InputButtonValue.Start]            = currentBindings.Start;
        keyLookup[InputButtonValue.Return]           = currentBindings.Return;
        keyLookup[InputButtonValue.Select]           = currentBindings.Select;
        keyLookup[InputButtonValue.Pause]            = currentBindings.Pause;
        keyLookup[InputButtonValue.Menu]             = currentBindings.Menu;
        keyLookup[InputButtonValue.Options]          = currentBindings.Options;
    }

    public void UpdateDevice()
    {
        UpdateAxis();   
        bool recievedButtonInput = this.UpdateButtons(buttonCount, keyLookup, ref buttonValues);
        if (recievedButtonInput)
            lastInputTime = Time.realtimeSinceStartup;
    }

    public void LateUpdateDevice()
    {
        throw new NotImplementedException();
    }

    private void UpdateAxis()
    {
        float lastAxisInputTime = 0;

        keyboardAxis.UpdateAxis();
        lastAxisInputTime = keyboardAxis.LastInputTime;
        UpdateAxisInputTime(lastAxisInputTime);

        mouseAxis.UpdateAxis();
        lastAxisInputTime = mouseAxis.LastInputTime;
        UpdateAxisInputTime(lastAxisInputTime);
        mouseWheelAxis.UpdateAxis();
        lastAxisInputTime = mouseWheelAxis.LastInputTime;
        UpdateAxisInputTime(lastAxisInputTime);
    }

    private void UpdateAxisInputTime(float lastAxisInput)
    {
        if (lastAxisInput > lastInputTime)
            lastInputTime = lastAxisInput;
    }

    public float GetAxis(InputAxisValue a_value)
    {
        switch (a_value)
        {
            case InputAxisValue.LeftX:
                return keyboardAxis.XAxis.Value;
            case InputAxisValue.LeftY:
                return keyboardAxis.YAxis.Value;
            case InputAxisValue.RightX:
                return mouseAxis.XAxis.Value;
            case InputAxisValue.RightY:
                return mouseAxis.YAxis.Value;
        }

        return 0f;
    }

    public int GetButton(InputButtonValue a_value)
    {
        return buttonValues[(int) a_value];
    }

    private void ResetButtons()
    {
        for (int i = 0; i < buttonValues.Length; i++)
        {
            buttonValues[i] = 0;
        }
    }
}
