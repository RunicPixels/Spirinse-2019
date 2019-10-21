using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Assets.Scripts;
namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(MonoBehaviour))]
    public class ClickableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            MonoBehaviour monoBehaviour = target as MonoBehaviour;

            GUILayout.BeginVertical();
            MethodInfo[] methods = monoBehaviour.GetType().GetMethods();
            foreach (MethodInfo info in methods)
            {
                Debug.Log(info.Name);

                ClickableAttribute attribute = info.GetCustomAttribute<ClickableAttribute>();
                
               // if (attribute != null)
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
