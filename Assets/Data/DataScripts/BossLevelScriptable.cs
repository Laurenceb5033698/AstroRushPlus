using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BossDataScriptableObject", order = 1)]
public class BossLevelScriptable : LevelDataScriptable
{
    public GameObject BossPrefab;
}
