using UnityEngine;

public class PickUpgradeState : StateTransition
{
    public void OnEnter()
    {
        ServicesManager.Instance.PauseService.Pause();
        UIManager.instance.UpgradeScreen();
        //normally functionality like this should not be called in OnEnter,
        //however this will randomise the cards everytime we go to pickupgrade, which is desired.
        (UIManager.instance.GetCurrentElement() as UI_Upgrade).GetRandomCardsFromPool();

    }

    public void OnExit()
    {

    }

    public void Update()
    {

    }
}
