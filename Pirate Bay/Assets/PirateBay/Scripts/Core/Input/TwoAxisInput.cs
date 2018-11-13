using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoAxisInput
{
    public OneAxisInput XAxis, YAxis;

    public float LastInputTime
    {
        get
        {
            float time = (XAxis.LastInputTime > YAxis.LastInputTime) ? XAxis.LastInputTime : YAxis.LastInputTime;
            return time;
        }
    }

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

public class TwoAxisWithButtonInput
{
    public OneAxisWithButtonInput XAxis, YAxis;

    public float LastInputTime
    {
        get
        {
            float time = (XAxis.LastInputTime > YAxis.LastInputTime) ? XAxis.LastInputTime : YAxis.LastInputTime;
            return time;
        }
    }

    public TwoAxisWithButtonInput(string a_xAxisName, string a_yAxisName)
    {
        XAxis = new OneAxisWithButtonInput(a_xAxisName);
        YAxis = new OneAxisWithButtonInput(a_yAxisName);
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