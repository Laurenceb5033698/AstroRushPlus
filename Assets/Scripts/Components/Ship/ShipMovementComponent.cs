using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// player and ai ship movement component.
/// moves gameobject around using spherecast? collision detection
/// 
/// </summary>
public class ShipMovementComponent : MonoBehaviour
{
    //current velocity of ship. affected by collisions
    Vector3 m_velocity;
    Rigidbody rb;
    [Range(0.1f, 20f)]
    [SerializeField] private float accelRatio = 8.0f;
    [Range (0.01f, 2f)]
    [SerializeField] private float dragRatio = 0.1f;
    [SerializeField] private float rotateAmount = 10.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //moves in several sections
    //1st: condition check
    //2nd: calc velocity (requires intent)
    //3rd: spherecast for collision
    //4th: move & collide


    //main entrypoint for moving ship per fixedupdate.
    //#Note# this WILL be called for drifting ships, must still collide and bounce etc.
    public void Motor(Vector3 _intendedVector, bool _isDrifting)
    {
        if (!CheckCondition())
            return;

        CalculateVelocity(_intendedVector, _isDrifting);

        //turn towards
        //float dotProd = Vector3.Dot(transform.forward, _intendedVector);
        //if (dotProd < 0.3f)
        //{
        //    //delta in direction is not close to forward
        //    //
        //}

        Quaternion facingVelocityRotation = Quaternion.LookRotation(m_velocity, Vector3.up);
        Quaternion newFacingRotation = Quaternion.RotateTowards(transform.rotation, facingVelocityRotation, rotateAmount);

        //move in velocity direction.
        //transform.position += m_velocity;
        rb.MovePosition(transform.position+m_velocity);
        rb.MoveRotation(newFacingRotation);

    }

    // if anything prevents movement
    public bool CheckCondition()
    {
        return true;
    }

    //using ship values, calculate velocity towards intended vector.
    //intended vector is given by ship controller/ai. 
    //not necessarily a target, but a direction and distance.
    public void CalculateVelocity(Vector3 _intendedVector, bool drifting)
    {
        //get ship values, for speed/accel/mass
        Stats shipStats = GetComponent<Stats>();
        float maxSpeed = shipStats.Get(StatType.sSpeed);
        float acceleration = shipStats.Get(StatType.sSpeed)/ accelRatio;

        if (_intendedVector != Vector3.zero || !drifting)
        {
            Vector3 delta = _intendedVector.normalized * acceleration;
            m_velocity += delta * Time.fixedDeltaTime;
            if(m_velocity.magnitude > maxSpeed)
            {
                m_velocity.Normalize();
                m_velocity *= maxSpeed;
            }
        }
        else
        {
            if (m_velocity.magnitude < 0.1f)
            {
                m_velocity = Vector3.zero;
            }
            else
            {
                Vector3 delta = m_velocity * dragRatio;
                m_velocity -= delta * Time.fixedDeltaTime;
            }
        }


    }

    //using unity physics collisions for simplicity.
    //do not need to consider colliding with ontriggers, as they will handle that
    private void OnCollisionEnter(Collision collision)
    {
        //when colliding: must change velocity for bump.

        
        m_velocity = collision.impulse / 2;
        //mv1 + mv2 = mv1' + mv2'
    }

}
