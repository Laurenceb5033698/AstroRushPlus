using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boid : MonoBehaviour
{
    //mass, pos, vel, orientation
    [SerializeField] public Vector3 velocity;
    [SerializeField] public Vector3 ;


    [SerializeField] public float maxSpeed = 50f;
    [SerializeField] public float maxForce = 5f;
    [SerializeField] public float innerRange = 30f;
    [SerializeField] public float OuterRange = 30f;

    public Boid target;



    protected Vector3 Seek()
    {
        Vector3 force = target.getPosition() - velocity;
        force.normalize();
        force *= maxSpeed;
        Vector3 steering = force - velocity;

        return Vector3.ClampMagnitude(steering, maxForce);
    }

    protected Vector3 Flee()
    {
        return -Seek();
    }

    protected void Pursue()
    {
        Vector3 force = target.getPosition() - velocity + target.getVelocity();
        force.normalize();
        force *= maxSpeed;
        Vector3 steering = force - velocity;

        return Vector3.ClampMagnitude(steering, maxForce);
    }

    protected void Evade()
    {
        return -Pursue();
    }

    protected void Arrive()
    {
        Vector3 force = target.getPosition() - velocity;// + target.getVelocity();
        Vector3 steering;
        float forceMag = force.magnitude;
        float distance = Vector3(target.getPosition() - getPosition()).magnitude;
        if( distance < innerRange)
        {
            float remainingDist = distance / innerRange;
            forceMag *= remainingDist;
            
        }

    }

    //
    public Vector3 getPosition()
    {
        return transform.position;
    }
    public Vector3 getVelocity()
    {
        return velocity;
    }
}