using UnityEngine;


/// <summary>
/// Factory for Creating a playership from provided data.
/// todo: upgrade to use selected loadout values.
/// </summary>
public class ShipFactory
{
    //Ship factory is created in the start function of GameManager and sets up the player ship gameobject
    //must be done in one go basically.
    //later this will change to use loadout values from player-selected loadout.
    private GameObject m_ShipPrefab;
    private Inputs m_Controls;

    public void SetVaraibles(GameObject _shipPrefab, Inputs _Controls)
    {
        m_ShipPrefab = _shipPrefab;
        m_Controls = _Controls;
    }

    public GameObject CreatePlayerShip()
    {
        //calls all awake functions.
        GameObject ship = GameManager.Instantiate(m_ShipPrefab, Vector3.zero, Quaternion.identity);

        //post spawn functions.
        ship.GetComponent<PlayerController>().SetInputs(m_Controls);
        UpgradePoolManager.instance.AssignPlayerStatsToUpgradeManager(ship);
        //applies loadout and ship base stats module.
        //  also required to correctly apply stats to starting health values.
        UpgradePoolManager.instance.GetComponent<UpgradeManager>().ApplyBaseStats();

        return ship;
    }




}
