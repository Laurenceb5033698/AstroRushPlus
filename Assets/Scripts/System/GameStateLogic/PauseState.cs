using UnityEngine;

public class PauseState : StateTransition
{
    public void OnEnter()
    {
        ServicesManager.Instance.PauseService.Pause();
        //UIManager.GetGameUiObject().Button_PausePressed(true);

    }

    public void OnExit()
    {

    }

    public void Update()
    {

    }
}
