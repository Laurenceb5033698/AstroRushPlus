using UnityEngine;

public class AI_Damageable : Damageable
{
    override public void TakeDamage(Vector3 _OtherPos, float _Amount)
    {
        GetComponentInParent<AICore>().TakeDamage(_OtherPos, _Amount);
    }
    public override Rigidbody GetRigidbody()
    {
        return GetComponentInParent<Rigidbody>();
    }
}
