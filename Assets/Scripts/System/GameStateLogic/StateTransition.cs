using UnityEngine;

/// <summary>
/// Abstract inteface for implementing game state logic.
/// </summary>
public interface StateTransition
{
    public void OnEnter();
    public void Update();
    public void OnExit();
}
