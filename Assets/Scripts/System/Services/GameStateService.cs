using UnityEngine;

[System.Serializable]
public enum GameState
{
    MENU,       //  in main menu scene.
    WARPIN,     //  in game scene, warp in anim.
    MAINGAME,   //  plaing game, things spawning, running
    PICKUPGRADE,//  end of wave, player picks upgrade
    WARPOUT,    //  after picking upgrade, warping to next level
    PAUSE,      //  mid game, paused by player, or gameover
    NONE        //out of bounds
}
/// <summary>
/// Gamestate is managed here.
/// Changing game state allows gamemanager to run different checks, control timescale, run different services, start or end waves.
/// </summary>
[System.Serializable]
public class GameStateService : IService
{
    [SerializeField]
    private GameState m_state;
    public GameState GameState { 
        get { return m_state; } 
        set { SetState(value); }
    }
    [SerializeField]
    private GameState m_PreviousGamestate = GameState.WARPIN;
    [SerializeField]
    StateTransition m_CurrentStateLogic = null;


    private void SetState(GameState _state)
    {
        if (_state < GameState.NONE)
        {
            m_state = _state;
            if (GameManager.instance != null) 
            {
                if(m_CurrentStateLogic != null)
                    m_CurrentStateLogic.OnExit();
                SetStateLogic(m_state);
                m_CurrentStateLogic.OnEnter();
            }

        }
        else
            Debug.Log("Error Gamestate invalid.");
    }

    public void GotoPreviousState()
    {
        GameState = m_PreviousGamestate;
    }

    public bool isState(GameState _stateTest)
    {
        return m_state == _stateTest;
    }

    public void SetStateLogic(GameState _newState)
    {
        switch (_newState)
        {
            case GameState.WARPIN:
                m_CurrentStateLogic = new EnterLevelState();
                m_PreviousGamestate = GameState.WARPIN;
                break;
            case GameState.MAINGAME:
                m_CurrentStateLogic = new MainGameState();
                m_PreviousGamestate = GameState.MAINGAME;
                break;
            case GameState.PICKUPGRADE:
                m_CurrentStateLogic = new PickUpgradeState();
                m_PreviousGamestate = GameState.PICKUPGRADE;
                break;
            case GameState.WARPOUT:
                m_CurrentStateLogic = new ExitLevelState();
                m_PreviousGamestate = GameState.WARPOUT;
                break;
            case GameState.PAUSE:
                m_CurrentStateLogic = new PauseState();
                //m_PreviousGamestate = GameState.PAUSE;
                break;
            case GameState.MENU:
                m_CurrentStateLogic = new MenuState();
                m_PreviousGamestate = GameState.MENU;
                break;
            default:
                Debug.LogError("Error GameStateService: Invalid Game State.");
                break;
        }

    }

    public void Update()
    {
        m_CurrentStateLogic.Update();
    }

    public void Initiallise()
    {
        GameState = GameState.MENU;
        m_PreviousGamestate = GameState.MENU;
        m_CurrentStateLogic = new MenuState();
        m_CurrentStateLogic.OnEnter();
    }

    public void Reset()
    {
        SetState(GameState.MENU);
    }
}