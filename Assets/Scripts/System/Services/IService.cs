using UnityEngine;

/// <summary>
/// Gameplay related services.
/// Implemented by singleton classes that are managed by ServiceManager.
/// </summary>
public interface IService
{
    /// <summary>
    /// called when first created. used to initiallise in awake state.
    /// </summary>
    public void Initiallise();
    //called whenever Service Manager is destroyed.
    public void Reset();
}
