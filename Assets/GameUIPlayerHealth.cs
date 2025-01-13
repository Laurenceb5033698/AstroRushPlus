using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class GameUIPlayerHealth : MonoBehaviour
{

    public PlayerController playerShip;
    public UIDocument GameUI;
    public VisualElement healthPipContainer;
    public VisualElement shieldBar;
    public VisualElement specialBar;
    public VisualElement missilePipsContainer;


    void Start()
    {
        healthPipContainer = GameUI.rootVisualElement.Q<VisualElement>("HealthPips");

        if(GameManager.instance != null)
        {
            playerShip = GameManager.instance.GetShipRef().GetComponent<PlayerController>();
        }
        if(playerShip == null)
        {
            Debug.LogError("GameUIPLayerHealth: no playerController assigned!");
        }
    }

    private void OnEnable()
    {   //add event when ui is created
        if (playerShip == null)
        {
            if (GameManager.instance != null)
            {
                playerShip = GameManager.instance.GetShipRef().GetComponent<PlayerController>();
            }
        }
        playerShip.OnHealthChanged += UpdateUI;
        playerShip.OnMaxStatsChanged += StatsChanged;

    }

    private void OnDisable()
    {   
        playerShip.OnHealthChanged -= UpdateUI;
        playerShip.OnMaxStatsChanged -= StatsChanged;
    }

    //when playership takes damage, this updates the ui without changing ui sizing ect
    void UpdateUI(Stats playerstats)
    {
        updateHealthPips(playerstats.Health.Max, playerstats.Health.Value);
    }

    //health can increase or decrease during gameplay.
    void updateHealthPips(int maxHP, int CurrHP)
    {
        //swaps active image using uss class styling.
        int index = 0;
        foreach (var item in healthPipContainer.Children())
        {
            if (index < CurrHP)
            {
                item.EnableInClassList(".HealthPipFull", true);
                item.EnableInClassList(".HealthPipEmpty", false);
            }
            else 
            {
                item.EnableInClassList(".HealthPipFull", false);
                item.EnableInClassList(".HealthPipEmpty", true);
            }
            index++;
        }
    }

    //##################

    //when Playership stats change (increase or decrease) including max amounts
    //refreshes existing pips aswell, could result in wierd scaling
    void StatsChanged(Stats playerStats)
    {
        SetHealthPips(playerStats.Health.Max, playerStats.Health.Value);
    }

    //remove and re-add all healthpips. sets css for 
    void SetHealthPips(int max, int current)
    {
        //remove all pips
        healthPipContainer.Clear();

        //add new pips up to new amount
        for ( int i = 0; i < max; i++)
        {
            VisualElement hpPip = new VisualElement();
            hpPip.name = "HealthPip";
            hpPip.AddToClassList(".HealthPip");
            hpPip.AddToClassList(".HealthPipFull:enabled");
            hpPip.AddToClassList(".HealthPipEmpty:disabled");
            healthPipContainer.Add(hpPip);
        }

        //assign css styling for pips
        //int index = 0;
        //foreach (var item in healthPipContainer.Children())
        //{
        //    if(index<current)
        //    {
        //        item.AddToClassList(".HealthPipFull");
        //    }
        //    else
        //    {
        //        item.AddToClassList(".HealthPipEmpty");
        //    }
        //    index++;
        //}

    }


}
