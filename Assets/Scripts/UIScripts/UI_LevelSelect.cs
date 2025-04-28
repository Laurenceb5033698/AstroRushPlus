using UnityEngine;

public class UI_LevelSelect : ScreenElement {


    private void Update()
    {
        HandleSubmit();
        HandleCancel();
        //up/down nav
        HandleNavigateUp();
        HandleNavigateDown();
    }

    protected override void Cancel()
    {
        Button_LevelSelectReturnPressed();
    }

    public void Button_AlphaLevelPressed()
    {
        UIManager.instance.StartGame(0);
    }
    public void Button_BetaLevelPressed()
    {
        UIManager.instance.StartGame(1);
    }
    public void Button_GammaLevelPressed()
    {
        UIManager.instance.StartGame(2);
    }

    public void Button_LevelSelectReturnPressed()
    {
        UIManager.instance.ReturnToMenu();
    }

}