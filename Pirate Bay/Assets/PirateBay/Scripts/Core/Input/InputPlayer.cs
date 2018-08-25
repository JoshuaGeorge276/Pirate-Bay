using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer
{
    public int ID;
    public InputController Controller;

    public bool isEngaged = false;

    public InputPlayer()
    {
    }

    public void Update(float a_fDeltaTime)
    {
        //if (!isEngaged)
        //{
        //    Controller.CheckEngagement();
        //}
        //else
        //{
            Controller.ResetController();
            Controller.UpdateController(a_fDeltaTime);
       // }
    }

    public void LateUpdate(float a_fDeltaTime)
    {

    }

    public void Engage(EngagementType a_type, int id = -1)
    {
        Controller = new InputController(a_type, id);
        isEngaged = true;
    }


}
