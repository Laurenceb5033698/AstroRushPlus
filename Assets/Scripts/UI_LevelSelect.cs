using UnityEngine;

public class UI_LevelSelect : ScreenElement {

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
        UIManager.instance.StartGame(3);
    }

    public void Button_LevelSelectReturnPressed()
    {
        UIManager.instance.ReturnToMenu();
    }

}