using UnityEngine;

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
public class GameStateService : IService
{
    private GameState m_state;
    public GameState GameState { 
        get { return m_state; } 
        set { SetState(value); }
    }

    


    private void SetState(GameState _state)
    {
        if (_state < GameState.NONE)
            m_state = _state;
        else
            Debug.Log("Error Gamestate invalid.");
    }

    public bool isState(GameState _stateTest)
    {
        return m_state == _stateTest;
    }


    public void Initiallise()
    {
        SetState(GameState.MENU);
    }

    public void Reset()
    {
        SetState(GameState.MENU);
    }
}