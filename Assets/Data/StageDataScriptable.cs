using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/StageDataScriptableObject", order = 1)]
public class StageDataScriptable : ScriptableObject
{

    public List<LevelDataScriptable> StageLevels;
    //maybe more information?
    //maybe boss level can be included?

}
