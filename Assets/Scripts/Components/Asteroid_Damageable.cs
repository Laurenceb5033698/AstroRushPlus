using UnityEngine;

public class Asteroid_Damageable : Damageable
{
    override public void TakeDamage(Vector3 _OtherPos, float _Amount)
    {
        GetComponent<Asteroid>().TakeDamage(_OtherPos, _Amount);
    }
    public override Rigidbody GetRigidbody()
    {
        return GetComponent<Rigidbody>();
    }
}
