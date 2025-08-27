using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Upgrade Card manager.
/// Handles inputs and card selection.
/// </summary>
public class UI_Upgrade : ScreenElement
{

    [SerializeField] private UpgradeCardDetailsHandler[] CardDetailsHandlers = new UpgradeCardDetailsHandler[3];
    public override void Update()
    {
        HandleSubmit();
        //HandleCancel();
        //left/right selecting
        HandleNavigateLeft();
        HandleNavigateRight();
    }

    protected override void Cancel()
    {
        //UIManager.instance.Resume();
    }

    //onsubmit - send card selection to player's upgrade manager
    //card selection must be removed from upgrade pools
    //may need to store selection to some easy to read pool for veiwing all upgrades screen later.

    /// <summary>
    /// card button invokes this.
    /// </summary>
    public void Button_CardPressedA()
    {
        //testing - return to game ui
        CardPressedGeneric(0);
    }
    public void Button_CardPressedB()
    {
        //testing - return to game ui
        CardPressedGeneric(1);
    }
    public void Button_CardPressedC()
    {
        //testing - return to game ui
        CardPressedGeneric(2);
    }

    private void CardPressedGeneric(int _card)
    {
        if (_card < 0)
            _card = 0;
        if (_card > 2)
            _card = 2;
        UpgradePoolManager.instance.PickUpgrade(_card);
        UIManager.instance.Resume();
        GameManager.instance.ResumeFromPickUpgrade();
    }

    //when enabled, rolls 3 cards to pick from.
    public void GetRandomCardsFromPool()
    {
        //get cards
        List<UModuleScriptable> randomCards = UpgradePoolManager.instance.SelectThreeUpgrade();
        //set display visuals and text
        for (int i = 0; i < 3; i++)
        {
            CardDetailsHandlers[i].SetDisplay(randomCards[i].DisplayDetails);
        }
    }
    //must load card data for desc, title, img, stats and set them on ui elements.
    //first time enabled picks from weapon types pool.
    //second time loading picks from missile types pool.
    //afterwards always picks from generic mixed upgrade pool + weapon type pool

    //gonna need some kinda pool manager for all that....
}
