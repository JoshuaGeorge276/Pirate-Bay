using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public IDeviceHandler Device;

    public InputController()
    {
        Device = new KeyboardHandler();
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
