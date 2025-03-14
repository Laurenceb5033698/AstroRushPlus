using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeManager))]
public class UpgradeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UpgradeManager upgradeManager = (UpgradeManager)target;
        if (GUILayout.Button("Add selected Upgrade"))
        {
            upgradeManager.TestAddModule();
        }
    }
}
