using UnityEngine;

public class Asteroid_Damageable : Damageable
{
    override public void TakeDamage(EventSource _offender, Vector3 _OtherPos, float _Amount)
    {
        GetComponent<Asteroid>().TakeDamage(_offender, _OtherPos, _Amount);
    }
    public override Rigidbody GetRigidbody()
    {
        return GetComponent<Rigidbody>();
    }
    public override EventSource GetEventSource()
    {
        return null;
    }
}
