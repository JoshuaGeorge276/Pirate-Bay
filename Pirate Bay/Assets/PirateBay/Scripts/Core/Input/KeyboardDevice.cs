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

    private float[] axisValues;
    private sbyte[] buttonValues;
    private Dictionary<InputAxisValue, string> axisLookup;
    private Dictionary<InputButtonValue, KeyCode> keyLookup;

    private Bindings currentBindings;

    private int buttonCount = (int) InputButtonValue.COUNT;

    public KeyboardDevice(Bindings a_bindings)
    {
        currentBindings = a_bindings;

        int axisCount = (int) InputAxisValue.COUNT;

        axisValues = new float[axisCount];
        buttonValues = new sbyte[buttonCount];
        
        axisLookup = new Dictionary<InputAxisValue, string>(axisCount);
        keyLookup = new Dictionary<InputButtonValue, KeyCode>(buttonCount);

        axisLookup[InputAxisValue.LeftX]             = currentBindings.LeftXAxis;
        axisLookup[InputAxisValue.LeftY]             = currentBindings.LeftYAxis;
        axisLookup[InputAxisValue.RightX]            = currentBindings.RightXAxis;
        axisLookup[InputAxisValue.RightY]            = currentBindings.RightYAxis;

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


        keyboardAxis = new TwoAxisInput(axisLookup[InputAxisValue.LeftX], axisLookup[InputAxisValue.LeftY]);
        mouseAxis = new TwoAxisInput(axisLookup[InputAxisValue.RightX], axisLookup[InputAxisValue.RightY]);
        mouseWheelAxis  = new OneAxisInput("Mouse ScrollWheel");
    }

    public void ResetDevice()
    {
        keyboardAxis.ResetAxis();
        mouseAxis.ResetAxis();
        mouseWheelAxis.ResetAxis();
        ResetButtons();
    }

    public void UpdateDevice()
    {
        UpdateAxis();   
        UpdateButtons();
    }

    public void LateUpdateDevice()
    {
        throw new NotImplementedException();
    }

    private void UpdateAxis()
    {
        keyboardAxis.UpdateAxis();
        mouseAxis.UpdateAxis();
        mouseWheelAxis.UpdateAxis();
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < buttonCount; ++i)
        {
            sbyte currentValue = buttonValues[i];
            KeyCode key = keyLookup[(InputButtonValue) i];

            if(currentValue != 0)
                Debug.Log("Break");

            switch (currentValue)
            {
                 case (sbyte) InputButtonState.None:

                     if (Input.GetKeyDown(key))
                         buttonValues[i] = (sbyte) InputButtonState.Down;

                     break;
                 case (sbyte) InputButtonState.Down:

                     if (Input.GetKey(key))
                     {
                         buttonValues[i] = (sbyte) InputButtonState.Pressed;
                     }
                     else if (Input.GetKeyUp(key))
                     {
                         buttonValues[i] = (sbyte) InputButtonState.Up;
                     }

                     break;
                 case (sbyte) InputButtonState.Pressed:

                     if (Input.GetKeyUp(key))
                         buttonValues[i] = (sbyte) InputButtonState.Up;

                     break;
                 case (sbyte) InputButtonState.Up:
                     
                     if (Input.GetKeyDown(key))
                         buttonValues[i] = (sbyte) InputButtonState.Down;

                     buttonValues[i] = (sbyte) InputButtonState.None;
                     break;
            }
        }
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

        return 0;
    }

    public int GetButton(InputButtonValue a_value)
    {
        return buttonValues[(int) a_value];
    }

    private void ResetButtons()
    {
        //for (int i = 0; i < buttonValues.Length; i++)
        //{
        //    buttonValues[i] = 0;
        //}
    }
}
