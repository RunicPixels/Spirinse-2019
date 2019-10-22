using UnityEngine;

namespace Assets.Scripts
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClickableAttribute : PropertyAttribute
    {
        public ClickableAttribute() { }
    }
}
