using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoAxisInput
{
    public OneAxisInput XAxis, YAxis;

    public TwoAxisInput(string a_XAxisName, string a_yAxisName)
    {
        XAxis = new OneAxisInput(a_XAxisName);
        YAxis = new OneAxisInput(a_yAxisName);
    }

    public void UpdateAxis()
    {
        XAxis.UpdateAxis();
        YAxis.UpdateAxis();
    }

    public void ResetAxis()
    {
        XAxis.ResetAxis();
        YAxis.ResetAxis();
    }
}
