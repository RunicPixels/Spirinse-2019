using UnityEngine;
using UnityEditor;


public class EnumFlagAttribute : PropertyAttribute
{
    public string name;

    public EnumFlagAttribute() { }

    public EnumFlagAttribute(string name)
    {
        this.name = name;
    }
}