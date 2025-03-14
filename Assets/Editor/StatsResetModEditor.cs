using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

//[CustomEditor(typeof(Stats))]
//public class StatsResetModEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        Stats myStats = (Stats)target;
//        if(GUILayout.Button("Reset All Mod and Bonus to 0"))
//        {
//            List<Stat> statList = myStats.EditorGetStatList();

//            //SerializedObject sObject = new UnityEditor.SerializedObject(myStats);
//            foreach (Stat stat in statList)
//            {
//                stat.SetShipMod(0);
//                stat.SetBonusMod(0);
//            }
            
//        }
//    }


//}
