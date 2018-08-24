using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class MethodUtil
{
    public static bool isOverridden(this MethodInfo methodInfo)
    {
        return (methodInfo.GetBaseDefinition() != methodInfo);
    }
}
