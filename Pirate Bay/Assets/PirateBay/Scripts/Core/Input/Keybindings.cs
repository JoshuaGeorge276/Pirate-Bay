using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindings : ScriptableObject {}

[CreateAssetMenu(menuName = "InputBindings/Keyboard", fileName = "Keyboard")]
public class KeyboardBindings : Bindings
{
    public string LeftXAxis, LeftYAxis;
    public string RightXAxis, RightYAxis;
    public string MouseWheelAxis;

    public KeyCode
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
        Options;
}

public enum GamePadKeyCode
{
    None,
    JoystickButton0,
    JoystickButton1,
    JoystickButton2,
    JoystickButton3,
    JoystickButton4,
    JoystickButton5,
    JoystickButton6,
    JoystickButton7,
    JoystickButton8,
    JoystickButton9,
    JoystickButton10,
    JoystickButton11,
    JoystickButton12,
    JoystickButton13,
    JoystickButton14,
    JoystickButton15,
    JoystickButton16,
    JoystickButton17,
    JoystickButton18,
    JoystickButton19
}

public static class GamePadKeyCodeHelper
{
    public static KeyCode GetTargetPadButton(GamePadKeyCode a_keyCode, int a_padID)
    {
        if (a_keyCode == GamePadKeyCode.None)
            return KeyCode.None;

        string name = a_keyCode.ToString();
        if (name.Length <= InputConst.GamePadKeyCodeSplitID)
            return KeyCode.None;

        a_padID += 1;
        name = name.Insert(InputConst.GamePadKeyCodeSplitID, a_padID.ToString());

        if (System.Enum.IsDefined(typeof(KeyCode), name))
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), name);
        }

        return KeyCode.None;
    }
}

[CreateAssetMenu(menuName = "InputBindings/GamePad", fileName = "GamePad")]
public class GamePadBindings : Bindings
{
    public string LeftXAxis, LeftYAxis;
    public string RightXAxis, RightYAxis;
    public string LeftTriggerAxis, RightTriggerAxis;
    public string DPadX, DPadY;

    public GamePadKeyCode
        LeftStickButton,
        RightStickButton,
        Action1,
        Action2,
        Action3,
        Action4,
        LeftBumper,
        RightBumper,
        Start,
        Return,
        Select,
        Pause,
        Menu,
        Options;
}