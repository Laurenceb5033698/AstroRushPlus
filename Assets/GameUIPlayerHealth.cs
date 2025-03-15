using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class GameUIPlayerHealth : MonoBehaviour
{

    public PlayerController playerShip;
    //public UIDocument GameUI;
    //public VisualElement healthPipContainer;
    //public VisualElement shieldBar;
    public VisualElement specialBar;
    public VisualElement missilePipsContainer;


    void Awake()
    {
    }

    //Gamemanager calls this via UIManager to set playership object and any other data
    public void onGameSceneLoaded(GameObject playerObject)
    {
        disconnectActions();
        playerShip = playerObject.GetComponent<PlayerController>();
        connectActions();
    }

    private void OnEnable()
    {
        //GameUI = GetComponent<UIDocument>();
        //healthPipContainer = GameUI.rootVisualElement.Q<VisualElement>("HealthPips");
        connectActions();

    }
    private void connectActions()
    {
        playerShip.OnHealthChanged += UpdateUI;
        playerShip.OnMaxStatsChanged += StatsChanged;
    }

    private void OnDisable()
    {
        disconnectActions();
    }

    private void disconnectActions()
    {
        if (playerShip != null)
        {
            playerShip.OnHealthChanged -= UpdateUI;
            playerShip.OnMaxStatsChanged -= StatsChanged;
        }
    }

    //when playership takes damage, this updates the ui without changing ui sizing ect
    void UpdateUI()
    {
        Stats playerstats = playerShip.gameObject.GetComponent<Stats>();
        updateHealthPips(playerstats.block.sHealth.Get(), playerstats.ShipHealth);
        updateShieldBar(playerstats.block.sShield.Get(), playerstats.ShipShield);
        updateAbilityBar(playerstats.block.sFuel.Get(), playerstats.ShipFuel);
    }

    //health can increase or decrease during gameplay.
    void updateHealthPips(float maxHP, float CurrHP)
    {
        VisualElement healthPipContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("HealthPips");
        //swaps active image using uss class styling.
        int index = 0;
        foreach (var item in healthPipContainer.Children())
        {
            if (index < CurrHP)
            {
                item.EnableInClassList("HealthPipFull", true);
                item.EnableInClassList("HealthPipEmpty", false);
            }
            else 
            {
                item.EnableInClassList("HealthPipFull", false);
                item.EnableInClassList("HealthPipEmpty", true);
            }
            index++;
        }
    }

    void updateShieldBar(float maxShield, float currShield)
    {
        VisualElement shieldBarmask = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("ShieldBarMask");

        float ShieldPercent = ((float)currShield / (float)maxShield)*100;
        //scale barmask width to shield percent
        shieldBarmask.style.width = Length.Percent(ShieldPercent);
    }

    void updateAbilityBar(float maxAbility, float currAbility)
    {
        VisualElement abilityBarmask = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("AbilityBarMask");

        float abilityPercent = ((float)currAbility / (float)maxAbility) * 100;
        //scale barmask width to shield percent
        abilityBarmask.style.width = Length.Percent(abilityPercent);
    }

    //##################

    //when Playership stats change (increase or decrease) including max amounts
    //refreshes existing pips aswell, could result in wierd scaling
    public void StatsChanged()
    {
        if (playerShip != null)
        {
            Stats playerStats = playerShip.gameObject.GetComponent<Stats>();
            SetHealthPips(playerStats.block.sHealth.Get(), playerStats.ShipHealth);
        }

        UpdateUI();
    }

    //remove and re-add all healthpips. sets css for 
    void SetHealthPips(float max, float current)
    {
        //remove all pips
        VisualElement healthPipContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("HealthPips");
        healthPipContainer.Clear();

        //add new pips up to new amount
        for ( int i = 0; i < max; i++)
        {
            VisualElement hpPip = new VisualElement();
            hpPip.name = "HealthPip";
            hpPip.AddToClassList("HealthPip");
            hpPip.AddToClassList("HealthPipFull:enabled");
            hpPip.AddToClassList("HealthPipEmpty:disabled");
            healthPipContainer.Add(hpPip);
        }
    }


}
