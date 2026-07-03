using UnityEngine;

/// <summary>
/// Part of the control of a state in any one of the microgames
/// </summary>
public abstract class GameState
{
    /// <summary>
    /// The actions to occur once a state is entered
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// The actions to occur every frame during the state
    /// </summary>
    /// <param name="dt">a reference to delta time</param>
    public abstract void Tick(float dt);

    /// <summary>
    /// The actions to occur once a state has finished
    /// </summary>
    public abstract void Exit();
}
