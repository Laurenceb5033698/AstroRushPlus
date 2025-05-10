using UnityEngine;

public class Player_Damageable : Damageable
{
    override public void TakeDamage(Vector3 _OtherPos, float _Amount)
    {
        GetComponent<PlayerController>().TakeDamage(_OtherPos, _Amount);
    }
}
