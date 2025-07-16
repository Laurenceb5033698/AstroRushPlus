using UnityEngine;
using System.Collections.Generic;

public class ServicesManager : MonoBehaviour 
{
    public static ServicesManager Instance;

    public HomingService HomingService { get { return Get<HomingService>() as HomingService; } private set { m_Services.Add(value);} }

    List<IService> m_Services;

    private void Awake()
    {
        //Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        m_Services = new List<IService>();

        //add services
        HomingService = new HomingService();

        

        //init services
        foreach(IService service in m_Services)
        {
            service.Initiallise();
        }
    }

    private IService Get<T>()
    {
        return m_Services.Find(x => x.GetType() == typeof(T));
    }

    private void OnDestroy()
    {
        foreach(IService service in m_Services)
        {
            service.Reset();
        }
    }

}
