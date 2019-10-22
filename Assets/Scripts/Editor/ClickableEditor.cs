using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Assets;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ClickableInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            MonoBehaviour monoBehaviour = target as MonoBehaviour;

            GUILayout.BeginVertical();
            MethodInfo[] methodInfo = target.GetType().GetMethods();
            foreach (MethodInfo info in methodInfo)
            {
                System.Attribute attribute = info.GetCustomAttribute(typeof(ClickableFunctionAttribute));
                
                if (attribute != null)
                {
                    if(GUILayout.Button(info.Name))
                    {
                        info.Invoke(monoBehaviour, null);
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }
}
