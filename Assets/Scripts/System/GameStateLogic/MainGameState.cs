using UnityEngine;

public class MainGameState : StateTransition
{
    //references to relevant objects.
    GameObject m_playerShip;
    AIManager m_aiManager;
    Pointer m_pointer;

    //states are setup with relevant data or references when created.
    public MainGameState()
    {
        GameManager gm = GameManager.instance;
        m_playerShip = gm.GetShipRef();
        m_aiManager = gm.gameObject.GetComponent<AIManager>();
        m_pointer = gm.pointer.GetComponent<Pointer>();
    }

    public void OnEnter()
    {
        ServicesManager.Instance.PauseService.Resume();
        if(m_aiManager.EndOfWave)
            m_aiManager.NewWave();
    }

    public void OnExit()
    {
        
    }

    public void Update()
    {
        if (!m_playerShip.GetComponent<Stats>().IsAlive())
        {
            //todo: maybe allow death sequence to play first
            //gamestate pause
            ServicesManager.Instance.GameStateService.GameState = GameState.PAUSE;
            //ServicesManager.Instance.PauseService.Pause();
            //change ui to gameover screen.
            UIManager.GetGameUiObject().Button_PausePressed(true);
            return;
        }
        if (m_aiManager.EndOfWave == true)
        {
            //UIManager.instance.UpgradeScreen();
            ServicesManager.Instance.GameStateService.GameState = GameState.PICKUPGRADE;
            //ServicesManager.Instance.PauseService.Pause();
            //m_aiManager.NewWave();
        }
        else
        {
            Vector3 ClosestEnemy = m_aiManager.GetClosestShipPos(m_playerShip.transform.position);
            m_pointer.SetNewTarget(ClosestEnemy);

            UpdateGameUI();
        }
    }
    private void UpdateGameUI()
    {
        if (!m_playerShip)
            return;
        UI_Game gamescreen = UIManager.GetGameUiObject();
        if (gamescreen)
        {
            gamescreen.UpdateShipStats( m_playerShip.GetComponent<Stats>() );
        }
    }
}
