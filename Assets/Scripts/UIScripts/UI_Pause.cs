using UnityEngine;

public class UI_Pause : ScreenElement
{
    [SerializeField]
    private GameObject MsgGameOver;
    [SerializeField]
    private GameObject MsgPaused;

    public override void Update()
    {
        HandleSubmit();
        HandleCancel();
        //up/down nav
        HandleNavigateUp();
        HandleNavigateDown();
    }

    protected override void Cancel()
    {
        Button_ContinuePressed();
    }

    public void Button_RestartPressed()
    {
        UpgradePoolManager.instance.RestartingGame();
        UIManager.instance.RestartLevel();
        //restarting causes scene to re-load.
        //this means gamemanager goes through awake-start again, which sets gamestate.

    }

    public void Button_ContinuePressed()
    {
        if (GameManager.instance.GetShipRef().GetComponent<Stats>().IsAlive())
        {
            ServicesManager.Instance.GameStateService.GotoPreviousState();
            UIManager.instance.Resume();
        }
    }

    public void Button_QuitToMenuPressed()
    {
        ServicesManager.Instance.GameStateService.GameState = GameState.MENU;
        UIManager.instance.EndLevel();

    }
    public void Button_OptionsPressed()
    {
        UIManager.instance.OptionsButton();
    }
    public void setMessage(bool _isPlayerDead)
    {
        if (_isPlayerDead)
        {
            MsgGameOver.SetActive(true);
            MsgPaused.SetActive(false);
        }
        else
        {
            MsgGameOver.SetActive(false);
            MsgPaused.SetActive(true);
        }
    }
}