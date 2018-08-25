using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public IDeviceHandler Device;

    public InputController(EngagementType a_type, int id = -1)
    {
        if(a_type == EngagementType.Keyboard)
            Device = new KeyboardHandler(InputManager.Instance.KeyboardDefaults());
        else if(a_type == EngagementType.Controller)
        {
            Device = new GamepadHandler(id, InputManager.Instance.GamePadDefaults());
        }
    }

    public bool CheckEngagement()
    {
        if (Input.anyKey)
        {
            //Debug.Log("Key Pressed");
            return true;
        }


        return false;
    }

    public void ResetController()
    {
        Device.ResetDevice();
    }

    public void UpdateController(float a_fDeltaTime)
    {
        Device.UpdateDevice();
    }
}
