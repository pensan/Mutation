// REDOX Game Labs 2016

#region INCLUDES
using UnityEngine;
using UnityEditor;
#endregion

[CustomEditor(typeof(TimeManager))]
public class TimeManagerInspector : Editor 
{
    private float customTime = 1.0f;
    public override void OnInspectorGUI()
    {
        TimeManager timeManger = (TimeManager)target;

        customTime = EditorGUILayout.Slider(timeManger.CustomTime, 0.0f, 10.0f);

        if (customTime != timeManger.CustomTime)
        {
            timeManger.CustomTime = customTime;
        }
    }     
}
