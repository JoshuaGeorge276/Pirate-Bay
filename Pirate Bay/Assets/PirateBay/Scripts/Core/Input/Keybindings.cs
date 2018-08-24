using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presets
{
    
    public static Bindings GetKeyboardBindings()
    {
        Bindings bindings = new Bindings
        {
            LeftXAxis = "Horizontal",
            LeftYAxis = "Vertical",
            RightXAxis = "Mouse X",
            RightYAxis = "Mouse Y",

            LeftStickButton = KeyCode.P,
            RightStickButton = KeyCode.Z,

            DPadUp = KeyCode.UpArrow,
            DPadDown = KeyCode.DownArrow,
            DPadLeft = KeyCode.LeftArrow,
            DPadRight =  KeyCode.RightArrow,

            Action1 = KeyCode.E,
            Action2 = KeyCode.Return,
            Action3 = KeyCode.Tab,
            Action4 = KeyCode.Space,

            LeftTrigger = KeyCode.None,
            RightTrigger = KeyCode.None,
            LeftBumper = KeyCode.None,
            RightBumper = KeyCode.None,
            Start = KeyCode.None,
            Return = KeyCode.None,
            Select =  KeyCode.None,
            Pause = KeyCode.None,
            Menu = KeyCode.None,
            Options = KeyCode.None
        };

        return bindings;
    }
}

[System.Serializable]
 public struct Bindings
 {
     public string LeftXAxis, LeftYAxis;
     public string RightXAxis, RightYAxis;
 
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
