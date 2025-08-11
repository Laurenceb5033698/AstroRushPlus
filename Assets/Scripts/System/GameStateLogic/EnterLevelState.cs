using UnityEngine;

public class EnterLevelState : StateTransition
{
    GameObject m_playerShip;

    float m_currentWarp;

    public EnterLevelState()
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
            //aiMngr.NewWave();
            //ResumingGame();
            //trigger ui for new level/objectives/hazards (depends on generated level)
            ServicesManager.Instance.GameStateService.GameState = GameState.MAINGAME;

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
