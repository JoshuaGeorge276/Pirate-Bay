using UnityEngine;

public class OneAxisInput
{
    private readonly string axisName;
    public float Value { get; private set; }

    public OneAxisInput(string a_name)
    {
        axisName = a_name;
    }

    public void UpdateAxis()
    {
        Value = Input.GetAxis(axisName);
    }

    public void ResetAxis()
    {
        Value = 0f;
    }
}

