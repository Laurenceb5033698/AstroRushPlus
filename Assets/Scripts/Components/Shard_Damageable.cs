using UnityEngine;

public class Shard_Damageable : Damageable
{
    override public void TakeDamage(Vector3 _OtherPos, float _Amount)
    {
        GetComponent<AsteroidShard>().TakeDamage(_OtherPos, _Amount);
    }
}
