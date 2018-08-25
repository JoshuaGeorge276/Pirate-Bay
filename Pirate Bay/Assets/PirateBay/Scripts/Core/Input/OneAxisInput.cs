using UnityEngine;

public class OneAxisInput
{
    private readonly string axisName;
    public float Value { get; private set; }

    public OneAxisInput(string a_name)
    {
        axisName = a_name;
    }

    public virtual void UpdateAxis()
    {
        Value = Input.GetAxis(axisName);
    }

    public virtual void ResetAxis()
    {
        Value = 0f;
    }
}

public class OneAxisWithButtonInput : OneAxisInput
{
    private sbyte keyValue;

    public OneAxisWithButtonInput(string a_name) : base(a_name){ }

    public override void UpdateAxis()
    {
        base.UpdateAxis();

        float axisValue = Mathf.Abs(Value);

        switch (keyValue)
        {
            case (sbyte)InputButtonState.None:

                if (axisValue > 0)
                    keyValue = (sbyte) InputButtonState.Down;
                break;

            case (sbyte)InputButtonState.Down:

                if (axisValue > 0)
                {
                    keyValue = (sbyte) InputButtonState.Pressed;
                }
                else
                {
                    keyValue = (sbyte) InputButtonState.None;
                }
                break;

            case (sbyte)InputButtonState.Pressed:

                if (axisValue <= 0)
                    keyValue = (sbyte) InputButtonState.Up;
                break;

            case (sbyte)InputButtonState.Up:

                if (axisValue > 0)
                    keyValue = (sbyte) InputButtonState.Down;

                keyValue = (sbyte) InputButtonState.None;
                break;
        }
    }

    public sbyte GetKeyValue()
    {
        return keyValue;
    }

    public sbyte GetPositiveKeyValue()
    {
        return Value > 0 ? keyValue : (sbyte) 0;
    }

    public sbyte GetNegativeKeyValue()
    {
        return Value < 0 ? keyValue : (sbyte) 0;
    }

    public override void ResetAxis()
    {
        base.ResetAxis();
        keyValue = 0;
    }
}

