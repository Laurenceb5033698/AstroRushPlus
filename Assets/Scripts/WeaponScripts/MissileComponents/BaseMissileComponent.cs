using UnityEngine;

public class BaseMissileComponent : MonoBehaviour
{
    protected BulletStats bulletStats;  //from projectile
    void Start()
    {
        bulletStats = GetComponent<Projectile>().m_Stats;
    }

    //when added to a missile, each component adds specific function to the gameobject.
    //  these functions are called by the missile projectile

    //Base class does nothing in each, but provides polymorphism for derived classes.
    //not abstract as dervied usaully only overrides one or two
    public virtual void OnInit() {}

    public virtual void PerUpdate() {}

    public virtual void PerFixed() {}

    public virtual void OnCollide() {}

}
