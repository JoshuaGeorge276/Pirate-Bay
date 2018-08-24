using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IDeviceHandler
{
    void ResetDevice();
    void UpdateDevice();
    void LateUpdateDevice();
    float GetAxis(InputAxisValue a_value);
    bool GetButtonDown(InputButtonValue a_value);
    bool GetButtonPressed(InputButtonValue a_value);
    bool GetButtonUp(InputButtonValue a_value);
}