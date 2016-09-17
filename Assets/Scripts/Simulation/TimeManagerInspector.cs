// REDOX Game Labs 2016

#region INCLUDES
using UnityEngine;
using UnityEditor;
#endregion

[CustomEditor(typeof(TimeManager))]
public class TimeManagerInspector : Editor 
{
    public override void OnInspectorGUI()
    {
        TimeManager timeManger = (TimeManager)target;

        timeManger.CustomTime = EditorGUILayout.Slider(timeManger.CustomTime, 1f, 10f);
    }     
}
