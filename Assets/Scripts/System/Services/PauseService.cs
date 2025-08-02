using UnityEngine;


/// <summary>
/// Manages Timescale for all gameplay uses.
/// </summary>
public class PauseService : IService
{
    private float m_Scale = 1;

    public float TimeScale
    {
        get { return m_Scale; }
        set { m_Scale = Mathf.Clamp01(value); SetTimeScale(m_Scale); }
    }

    public void Initiallise()
    {
        SetTimeScale(1);
    }

    public void Reset()
    {
        SetTimeScale(1);
    }



    public void Resume()
    {
        //setting property updates Time.timescale
        TimeScale = 1;
    }
    public void Pause()
    {
        TimeScale = 0;
    }

    private void SetTimeScale(float _timeScale)
    {
        Time.timeScale = Mathf.Clamp01(_timeScale);
    }

    //maybe can use coroutine to lerp timescale, or add to some slow-down amount for gameplay time mechanic.
    //needs to be an override so game-related pause cannot interrupt menu-related pause

    public bool PauseState()
    {
        return m_Scale == 0f;
    }
}
