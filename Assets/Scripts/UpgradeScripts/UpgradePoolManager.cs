using UnityEngine;
using System.Collections.Generic;


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

    //need to store which missile types are already picked, so we can pick from them later.
    public List<UModuleScriptable> UnselectedMissileTypes;

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
        Missile,
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
        if(Data_MissilePool)
            UnselectedMissileTypes = new List<UModuleScriptable>(Data_MissilePool.Pool);

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
            case PickState.Missile:
                if (UnselectedMissileTypes.Count > 0) WorkingPool.AddRange(UnselectedMissileTypes);
                break;
            case PickState.Standard:
                if (Data_GenericWeaponPool) WorkingPool.AddRange(Data_GenericWeaponPool.Pool);
                if (Data_GenericShipPool) WorkingPool.AddRange(Data_GenericShipPool.Pool);
                if (UnselectedMissileTypes.Count > 0) WorkingPool.AddRange(UnselectedMissileTypes);
                if (Data_GenericMissilePool) WorkingPool.AddRange(Data_GenericMissilePool.Pool);
                if (Data_SpecificPool) WorkingPool.AddRange(Data_SpecificPool.Pool);
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
                int cardindex = UnityEngine.Random.Range(0, WorkingPool.Count);
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
                int cardindex = UnityEngine.Random.Range(0, WorkingPool.Count);
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

        //if we pick a missile type upgrade (eg homing, nuke, salvo) we must remove it from the missile types list.
        RemoveMissileTypeFromPool(_SelectedCardIndex);

        //chosen upgrade is removed.
        SelectedUpgrades.RemoveAt(_SelectedCardIndex);

        //unchosen upgrades go back into pool.
        WorkingPool.AddRange(SelectedUpgrades);

        //clear selection and try advance pickstate
        SelectedUpgrades.Clear();
        ChangePickState();
    }

    private void ChangePickState()
    {
        //once weapon picked, change to next pool.
        if (CurrentPick == PickState.WeaponType)
        {
            CurrentPick = PickState.Missile;
            //only refresh if pickState changed.
            RefreshPool();
        }
        else
        {
            if(CurrentPick == PickState.Missile)
            {
                CurrentPick = PickState.Standard;
                RefreshPool();
            }
        }
    }

    /// <summary>
    /// If missile Type is picked, remove missile types from its own pool.
    /// </summary>
    /// <param name="_SelectedCardIndex"></param>
    private void RemoveMissileTypeFromPool(int _SelectedCardIndex)
    {
        if (SelectedUpgrades[_SelectedCardIndex].DisplayDetails.Type == CardType.Missile)
        {
            int upgradeIndex = UnselectedMissileTypes.IndexOf(SelectedUpgrades[_SelectedCardIndex]);
            if (upgradeIndex >= 0)
            {
                UnselectedMissileTypes.RemoveAt(upgradeIndex);
            }
        }
    }

    public void RestartingGame()
    {
        WorkingPool.Clear();
        InitialiseWorkingPools();

        GetComponent<UpgradeManager>().RestartingGame();
    }

    //replaces specific pool when a new weapon type is chosen.
    public void SetSpecificWeaponPool(ModulePoolScriptable _specificPoolScriptable)
    {
        Data_SpecificPool = _specificPoolScriptable;
    }
}
