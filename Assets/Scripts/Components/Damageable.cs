using UnityEngine;


/// <summary>
/// Base class for interface to allow gameobjects to take damage.
/// </summary>
abstract public class Damageable : MonoBehaviour
{
    abstract public void TakeDamage(EventSource _offender, Vector3 _OtherPos, float _Amount);
    abstract public Rigidbody GetRigidbody();
    abstract public EventSource GetEventSource();
}
