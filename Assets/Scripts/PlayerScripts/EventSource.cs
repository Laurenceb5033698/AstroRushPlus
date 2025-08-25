using UnityEngine;

/// <summary>
/// EventSource is a component responsible for holding all event delegates that an actor can call.
///     if a bullet this ship shot kills an enemyship, the enemy ship will call the onkillEvent action.
///     this triggers all subscribers of the onkillEvent
/// EventSource can be passed to projectiles, so that a hit target can use the methods provided.
/// </summary>
public class EventSource : MonoBehaviour
{
    public delegate void Interaction(GameObject target);
    public Interaction OnKill;
    public Interaction OnDamage;
    public Interaction OnCollide;

    //A target was killed by this objects damage.
    public void OnKillEvent(GameObject _object)
    {
        OnKill(_object);
    }
    //A target was Damaged, but not killed by this object's damage.
    public void OnDamageEvent(GameObject _object) 
    {
        OnDamage(_object); 
    }
    //A target was collided with by this object, but not killed.
    public void OnCollideEvent(GameObject _object)
    {
        OnCollide(_object);
    }


    public void AttachEventGeneric(BuffCondition _condition, Interaction _ActionFunction)
    {
        switch (_condition)
        {
            case BuffCondition.OnPickup:
                Debug.Log("EventSource Attach: onPickup Condition Event not Implemented.");
                break;
            case BuffCondition.OnKill:
                OnKill += _ActionFunction;
                break;
            case BuffCondition.OnDamage:
                OnDamage += _ActionFunction;
                break;
            case BuffCondition.OnShoot:
                Debug.Log("EventSource Attach: onShoot Condition Event not Implemented.");
                break;
            case BuffCondition.OnCollide:
                OnCollide += _ActionFunction;
                break;
            default:
                Debug.Log("EventSource Attach: invalid event condition.");
                break;
        }
    }

    public void DetachEventGeneric(BuffCondition _condition, Interaction _ActionFunction)
    {
        switch (_condition)
        {
            case BuffCondition.OnPickup:
                Debug.Log("EventSource Attach: onPickup Condition Event not Implemented.");
                break;
            case BuffCondition.OnKill:
                OnKill -= _ActionFunction;
                break;
            case BuffCondition.OnDamage:
                OnDamage -= _ActionFunction;
                break;
            case BuffCondition.OnShoot:
                Debug.Log("EventSource Attach: onShoot Condition Event not Implemented.");
                break;
            case BuffCondition.OnCollide:
                OnCollide -= _ActionFunction;
                break;
            default:
                Debug.Log("EventSource Attach: invalid event condition.");
                break;
        }
    }

}
