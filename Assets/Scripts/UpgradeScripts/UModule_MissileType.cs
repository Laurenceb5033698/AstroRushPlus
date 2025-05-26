using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// extends upgrade module to add missile specific type upgrades.
/// derived classes define specific behaviours for that missile upgrade.
/// </summary>
[System.Serializable]
public class UModule_MissileType : UpgradeModule
{

    //a missile type upgrade will usually involve adding a component to the missile using a delegate as it spawns.
    [SerializeField] public BaseMissileComponent m_baseComponent;

    //ctor from scriptable object data
    public UModule_MissileType(List<Stat> _list, BaseMissileComponent _prefab)
        : base(_list)
    {
        m_baseComponent = _prefab;
    }


    protected override void AttachCallbacks(GameObject _shipObject)
    {
        //get equipment component on playership
        Equipment shipEquipment = _shipObject.GetComponentInChildren<Equipment>();
        if (shipEquipment != null)
        {
            shipEquipment.RegisterToMissileUpgrader(SpawnCallback);
        }
    }

    //this function definition must match delegate in Equipment.
    protected virtual void SpawnCallback(GameObject _missileObject)
    {
        //add specific functionality in derived class.
        BaseMissileComponent[] attachedComponents = _missileObject.GetComponents<BaseMissileComponent>();

        foreach (BaseMissileComponent attachedComponent in attachedComponents)
        {
            if (attachedComponent.GetType() == m_baseComponent.GetType())
            {
                //already got one, donot attach.
                return;
            }
        }

        //dont have one yet, add to missile.
        _missileObject.AddComponent(m_baseComponent.GetType());
    }
}
