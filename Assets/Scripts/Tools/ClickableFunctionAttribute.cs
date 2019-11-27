using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class ClickableFunctionAttribute : PropertyAttribute
{
    public ClickableFunctionAttribute()
    {
    }
}