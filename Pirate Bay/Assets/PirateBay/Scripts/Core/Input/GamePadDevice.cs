using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GamePadDevice : IInputDevice
{
    private int PadID;
    private TwoAxisInput LeftStickAxis;
    private TwoAxisInput RightStickAxis;
    private TwoAxisWithButtonInput DPadAxis;
    private OneAxisWithButtonInput LeftTrigger, RightTrigger;

    private sbyte[] buttonValues;
    private Dictionary<InputButtonValue, KeyCode> keyLookup;

    private GamePadBindings currentBindings;

    private int buttonCount = (int) InputButtonValue.COUNT;

    public GamePadDevice(GamePadBindings a_bindings, int a_id)
    {
        PadID = a_id;
        currentBindings = a_bindings;

        buttonValues = new sbyte[buttonCount];

        keyLookup = new Dictionary<InputButtonValue, KeyCode>(buttonCount);

        SetupDevice(currentBindings);
    }

    public void ResetDevice()
    {
        LeftStickAxis.ResetAxis();
        RightStickAxis.ResetAxis();
        DPadAxis.ResetAxis();
        LeftTrigger.ResetAxis();
        RightTrigger.ResetAxis();
        ResetButtons();
    }

    public void SetupDevice(Bindings a_bindings)
    {
        LeftStickAxis  = new TwoAxisInput(currentBindings.LeftXAxis + PadID,
                                          currentBindings.LeftYAxis + PadID);

        RightStickAxis = new TwoAxisInput(currentBindings.RightXAxis + PadID,
                                          currentBindings.RightYAxis + PadID);

        DPadAxis       = new TwoAxisWithButtonInput(currentBindings.DPadX + PadID,
                                          currentBindings.DPadY + PadID);

        LeftTrigger = new OneAxisWithButtonInput(currentBindings.LeftTriggerAxis + PadID);
        RightTrigger = new OneAxisWithButtonInput(currentBindings.RightTriggerAxis + PadID);

        keyLookup[InputButtonValue.LeftStickButton]  = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.LeftStickButton, PadID);
        keyLookup[InputButtonValue.RightStickButton] = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.RightStickButton, PadID);
        
        keyLookup[InputButtonValue.Action1]          = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Action1, PadID);
        keyLookup[InputButtonValue.Action2]          = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Action2, PadID);
        keyLookup[InputButtonValue.Action3]          = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Action3, PadID);
        keyLookup[InputButtonValue.Action4]          = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Action4, PadID);

        keyLookup[InputButtonValue.LeftBumper]       = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.LeftBumper, PadID);
        keyLookup[InputButtonValue.RightBumper]      = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.RightBumper, PadID);

        keyLookup[InputButtonValue.Start]            = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Start, PadID);
        keyLookup[InputButtonValue.Return]           = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Return, PadID);
        keyLookup[InputButtonValue.Select]           = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Select, PadID);
        keyLookup[InputButtonValue.Pause]            = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Pause, PadID);
        keyLookup[InputButtonValue.Menu]             = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Menu, PadID);
        keyLookup[InputButtonValue.Options]          = GamePadKeyCodeHelper.GetTargetPadButton(currentBindings.Options, PadID);
    }

    public void UpdateDevice()
    {
        UpdateAxis();
        this.UpdateButtons(buttonCount, keyLookup, ref buttonValues);
    }

    public void LateUpdateDevice()
    {
        throw new System.NotImplementedException();
    }

    private void UpdateAxis()
    {
        LeftStickAxis.UpdateAxis();
        RightStickAxis.UpdateAxis();
        DPadAxis.UpdateAxis();
        LeftTrigger.UpdateAxis();
        RightTrigger.UpdateAxis();
    }

    public float GetAxis(InputAxisValue a_value)
    {
        switch (a_value)
        {
                case InputAxisValue.LeftX:
                    return LeftStickAxis.XAxis.Value;
                case InputAxisValue.LeftY:
                    return LeftStickAxis.YAxis.Value;
                case InputAxisValue.RightX:
                    return RightStickAxis.XAxis.Value;
                case InputAxisValue.RightY:
                    return RightStickAxis.YAxis.Value;
        }

        return 0f;
    }

    public int GetButton(InputButtonValue a_value)
    {
        switch (a_value)
        {
                case InputButtonValue.LeftTrigger:
                    return LeftTrigger.GetKeyValue();
                case InputButtonValue.RightTrigger:
                    return RightTrigger.GetKeyValue();
                case InputButtonValue.DPadUp:
                    return DPadAxis.YAxis.GetPositiveKeyValue();
                case InputButtonValue.DPadDown:
                    return DPadAxis.YAxis.GetNegativeKeyValue();
                case InputButtonValue.DPadLeft:
                    return DPadAxis.XAxis.GetPositiveKeyValue();
                case InputButtonValue.DPadRight:
                    return DPadAxis.XAxis.GetNegativeKeyValue();
                default:
                    return buttonValues[(int) a_value];
        }
    }

    private void ResetButtons()
    {
        for (int i = 0; i < buttonValues.Length; i++)
        {
            buttonValues[i] = 0;
        }
    }



}
