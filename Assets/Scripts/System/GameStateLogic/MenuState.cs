using UnityEngine;

public class MenuState : StateTransition
{
    public void OnEnter()
    {
        ServicesManager.Instance.PauseService.Resume();
    }

    public void OnExit()
    {

    }

    public void Update()
    {

    }
}
