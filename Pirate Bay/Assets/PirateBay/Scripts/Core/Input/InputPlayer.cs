using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer
{
    public int ID;
    public InputController Controller;

    public InputPlayer()
    {
        Controller = new InputController();
    }

    public void Update(float a_fDeltaTime)
    {
        Controller.ResetController();
        Controller.UpdateController(a_fDeltaTime);
    }

    public void LateUpdate(float a_fDeltaTime)
    {

    }
}
