using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(Stats))]
public class StatsResetModEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Stats myStats = (Stats)target;
        if (GUILayout.Button("Recalculate all stats for inspector changes"))
        {
            StatBlock block = myStats.block;
            block.RecalculateStats();
        }
    }
}
