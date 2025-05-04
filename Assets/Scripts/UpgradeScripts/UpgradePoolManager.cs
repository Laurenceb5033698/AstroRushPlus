using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Contains lists of all upgrades for each of the types of upgrade available.
/// Maintains working lists of available upgrades as theyre picked and removed from pools.
/// Can return select random upgrades for cards depending on mode
/// </summary>
public class UpgradePoolManager : MonoBehaviour
{
    public static UpgradePoolManager instance = null;

    //refereneces to module Pools
    [SerializeField] private ModulePoolScriptable Data_WeaponPool;  //types of weapons
    [SerializeField] private ModulePoolScriptable Data_MissilePool; //types of missiles
    [SerializeField] private ModulePoolScriptable Data_GenericShipPool;     //every generic Ship upgrade
    [SerializeField] private ModulePoolScriptable Data_GenericWeaponPool;   //every generic Weapon upgrade
    [SerializeField] private ModulePoolScriptable Data_GenericMissilePool;  //every generic Missile upgrade
    [SerializeField] private ModulePoolScriptable Data_SpecificPool;        //specific pool for the kind of weapon chosen. (set after weapon is picked)

    //Working list of available upgrades
    public List<UModuleScriptable> WorkingPool;

    public List<UModuleScriptable> SelectedUpgrades;


    private void Awake()
    {
        //create singleton
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        //makes upgrade pool persist between scene changes.
        DontDestroyOnLoad(gameObject);
    }

    enum PickState
    {
        WeaponType,
        //Missile,
        Standard
    };
    private PickState CurrentPick;

    //start  - created on object for this run. on restart - reinitialise working pools
    private void Start()
    {
        InitialiseWorkingPools();
    }

    public void AssignPlayerStatsToUpgradeManager(GameObject _PlayerObject)
    {
        Stats playerstats =  _PlayerObject.GetComponent<Stats>();
        if (playerstats == null)
        {
            Debug.Log("Playerstats null on assigning to upgrade module");
            return;
        }
        GetComponent<UpgradeManager>().shipStats = playerstats;
    }

    private void InitialiseWorkingPools()
    {
        WorkingPool = new List<UModuleScriptable>();
        //clear working pool, reset with weapon types selesction
        CurrentPick = PickState.WeaponType;
        RefreshPool();
    }

    private void RefreshPool()
    {
        WorkingPool.Clear();

        //for given pick state, refresh from data pools
        switch (CurrentPick) {
            case PickState.WeaponType:
                WorkingPool = new List<UModuleScriptable>(Data_WeaponPool.Pool);
                break;
            case PickState.Standard:
                if (Data_GenericWeaponPool) WorkingPool.AddRange(Data_GenericWeaponPool.Pool);
                if (Data_GenericShipPool) WorkingPool.AddRange(Data_GenericShipPool.Pool);
                if (Data_GenericMissilePool) WorkingPool.AddRange(Data_GenericMissilePool.Pool);
                break;

        }

    }


    /// <summary>
    /// Returns three random upgrade options from working pool
    /// </summary>
    public List<UModuleScriptable> SelectThreeUpgrade()
    {
        if (SelectedUpgrades.Count > 0)
            SelectedUpgrades.Clear();

        if(WorkingPool.Count >=3)
        {
            for (int i = 0; i < 3; i++)
            {
                int cardindex = Random.Range(0, WorkingPool.Count);
                SelectedUpgrades.Add(WorkingPool[cardindex]);
                //slow, but necessary to remove now to prevent repeat pulls.
                WorkingPool.RemoveAt(cardindex);
            }
        }
        else
        {
            //working pool has one or two upgrades remaining in pool. add any to selected,
            //then refresh pool and let an existing one be picked again
            foreach ( UModuleScriptable module in WorkingPool )
            {
                SelectedUpgrades.Add(module);
            }
            //lots of list adding, could be optimised.
            RefreshPool();
            for (int i = SelectedUpgrades.Count; i < 3; i++)
            {
                int cardindex = Random.Range(0, WorkingPool.Count);
                SelectedUpgrades.Add(WorkingPool[cardindex]);
                //slow, but necessary to remove now to prevent repeat pulls.
                WorkingPool.RemoveAt(cardindex);
            }

        }


        if (SelectedUpgrades.Count < 3)
            Debug.Log("less than 3 cards selected!");

        return SelectedUpgrades;
    }

    /// <summary>
    /// applies picked upgrade to player ship's upgrade manager.
    /// Called from upgrade UI screen.
    /// </summary>
    public void PickUpgrade(int _SelectedCardIndex)
    {
        //get player upgrademanager
        //GameObject playerShip = GameManager.instance.GetShipRef();
        UpgradeManager PlayerUpgradeManager = GetComponent<UpgradeManager>();
        //hand add upgrade from selectedUpgrades at selected index
        PlayerUpgradeManager.AddNewModule(SelectedUpgrades[_SelectedCardIndex]);

        SelectedUpgrades.Clear();
        ChangePickState();
    }

    private void ChangePickState()
    {
        //once weapon picked, change to next pool.
        if (CurrentPick == PickState.WeaponType)
            CurrentPick = PickState.Standard;
        RefreshPool();
    }

}
