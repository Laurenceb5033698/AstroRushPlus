using UnityEngine;
using System.Collections.Generic;

public class UModule_Salvo : UpgradeModule
{
    public UModule_Salvo(List<Stat> _list) : base(_list)
    {
    }

    protected override void AttachCallbacks(GameObject _shipObject)
    {
        _shipObject.GetComponentInChildren<Equipment>().ChangeOrdinanceProjectileSpawner();
    }
}
