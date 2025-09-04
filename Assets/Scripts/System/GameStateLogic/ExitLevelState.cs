using UnityEngine;

public class ExitLevelState : StateTransition
{
    GameObject m_playerShip;

    float m_currentWarp;


    public ExitLevelState()
    {
        GameManager gm = GameManager.instance;
        m_playerShip = gm.GetShipRef();
        m_currentWarp = Time.time + gm.m_WarpinTime;
    }

    public void OnEnter()
    {
        ServicesManager.Instance.PauseService.Resume();
    }

    public void OnExit()
    {
    }

    public void Update()
    {
        UpdateGameUI();
        if (Time.time > m_currentWarp)
        {
            //playing full warp effect.
            //  warp effect palys, but differnt wave data loaded, bg changes, ui updated, but same scene
            GameManager.instance.EndLevel();
            ServicesManager.Instance.GameStateService.GameState = GameState.WARPIN;

        }
    }

    private void UpdateGameUI()
    {
        if (!m_playerShip)
            return;
        UI_Game gamescreen = UIManager.GetGameUiObject();
        if (gamescreen)
        {
            gamescreen.UpdateShipStats(m_playerShip.GetComponent<Stats>());
        }
    }
}
