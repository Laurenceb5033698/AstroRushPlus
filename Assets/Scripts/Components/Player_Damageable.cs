using UnityEngine;

public class Player_Damageable : Damageable
{
    override public void TakeDamage(Vector3 _OtherPos, float _Amount)
    {
        GetComponentInParent<PlayerController>().TakeDamage(_OtherPos, _Amount);
    }

    public override Rigidbody GetRigidbody()
    {
        return GetComponentInParent<Rigidbody>();
    }
}
