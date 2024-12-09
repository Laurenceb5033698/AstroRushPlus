using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDataScriptableObject", order = 1)]
public class LevelDataScriptable : ScriptableObject
{
    public List<GameObject> NormalShipPrefabs;
    public List<GameObject> EliteShipPrefabs;
    public int TotalShips = 10;
    public int MaxActiveShip = 10;
    public float Difficulty = 0.0f;


}
