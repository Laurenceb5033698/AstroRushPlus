using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    public void Button_ContinuePressed()
    {
        if (GameManager.instance.GetShipRef().GetComponent<Stats>().IsAlive() )
            UIManager.instance.Resume(true);
    }

    public void Button_QuitToMenuPressed()
    {
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