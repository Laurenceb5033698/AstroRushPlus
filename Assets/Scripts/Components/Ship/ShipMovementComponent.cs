using UnityEngine;

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
    ShipRotationHandler tilthandler;
    [Range(0.1f, 20f)]
    [SerializeField] private float accelRatio = 8.0f;
    [Range (0.01f, 2f)]
    [SerializeField] private float dragRatio = 0.1f;
    [SerializeField] private float rotatefactor = 1.0f;
    [SerializeField] private float rotateMaxMag = 0.1f;
    [SerializeField] private float maxTurnrate = 1.0f;

    public float SpeedModifier = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tilthandler = GetComponentInChildren<ShipRotationHandler>();

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
        Stats shipStats = GetComponent<Stats>();
        float maxSpeed = shipStats.Get(StatType.sSpeed);
        //Vector3 angleDelta = _intendedVector.normalized - m_velocity.normalized;


        CalculateVelocity(_intendedVector.normalized, _intendedVector.magnitude, _isDrifting);

        Quaternion facingVelocityRotation = Quaternion.LookRotation(m_velocity.normalized, Vector3.up);
        Quaternion newFacingRotation = Quaternion.RotateTowards(transform.rotation, facingVelocityRotation, maxTurnrate);
       
        rb.linearVelocity = m_velocity;
        rb.MoveRotation(newFacingRotation);
        
        float newSpeed = m_velocity.magnitude;

        //calculate tilt amount
        //if speed > amount then tilt more
        float tiltModifier = 0.5f;
        if (newSpeed > maxSpeed / 2)
        {
            tiltModifier = newSpeed / maxSpeed;
        }
        //rotate visualrig by tilt amount
        tilthandler.Tilt(_intendedVector.normalized, tiltModifier);

    }

    // if anything prevents movement
    public bool CheckCondition()
    {
        return true;
    }

    //using ship values, calculate velocity towards intended vector.
    //intended vector is given by ship controller/ai. 
    //not necessarily a target, but a direction and distance.

    //input direction is normalized. inputspeed separate
    public void CalculateVelocity(Vector3 _inputDirection, float _speedPercent, bool drifting)
    {
        //get ship values, for speed/accel/mass
        Stats shipStats = GetComponent<Stats>();
        float maxSpeed = shipStats.Get(StatType.sSpeed) * SpeedModifier;
        float acceleration = (maxSpeed)/ accelRatio;

        float oldSpeed = m_velocity.magnitude;
        float targetSpeed = maxSpeed * _speedPercent;
        float delta = 0;
        if (targetSpeed >= oldSpeed)
        {
            //speed up
            
            delta += Mathf.Min(targetSpeed - oldSpeed, acceleration); ;
        }
        else
        {
            delta -= Mathf.Min(oldSpeed - targetSpeed, acceleration); ;
        }
        //turning should be able to maintain top speed.
        //you move by the magnitude of input(capped), but turn towards input over time
        //acceleration is the time to top speed, but does not affect turn speed, or speed during turns

        if (_inputDirection != Vector3.zero || !drifting)
        {
            m_velocity += transform.forward * delta * Time.deltaTime;
            float newSpeed = m_velocity.magnitude;

            //180deg in 1s
            const float step = Mathf.PI / 60;
            float maxRotateDelta = rotatefactor * step;
            Vector3 targetVector = _inputDirection * newSpeed;

            

            m_velocity = Vector3.RotateTowards(m_velocity, targetVector, maxRotateDelta, rotateMaxMag);

            //cap velocity
            if(newSpeed > maxSpeed)
            {
                m_velocity.Normalize();
                m_velocity *= maxSpeed;
            }
        }
        else
        {
            //no input, or drifting. slow down
            if (m_velocity.magnitude < 0.1f)
            {
                m_velocity = Vector3.zero;
            }
            else
            {
                Vector3 driftdrag = m_velocity * dragRatio;
                m_velocity -= driftdrag * Time.fixedDeltaTime;
            }
        }


    }

    //using unity physics collisions for simplicity.
    //do not need to consider colliding with ontriggers, as they will handle that
    private void OnCollisionEnter(Collision collision)
    {
        //when colliding: must change velocity for bump.
        //float massRatio = collision.rigidbody.mass / rb.mass;

        //m_velocity = collision.relativeVelocity * massRatio;
        //mv1 + mv2 = mv1' + mv2'
    }

}
