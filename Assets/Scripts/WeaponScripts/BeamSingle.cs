using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSingle : Weapon_Cannon
{
    //Beam-Single Weapon:
    //  Casts a Ray each tick, the first taget found marks the distance for the beam
    //  uses line renderer? for gfx
    //  gets collision object of first target and inflicts damage to that single target
    public LineRenderer lr;
    public float BeamRange = 40f;
    bool did_shoot =false;
    public GameObject psImpactPrefab;//particleSystem prefab
    GameObject psEffect;

    private void Start()
    {
        psEffect = Instantiate(psImpactPrefab, ship.transform.position, ship.transform.rotation);
        lr.enabled = false;
    }

    new private void OnDisable()
    {
        base.OnDisable();
        lr.enabled = false;
    }

    private void Update()
    {
        //while enabled: every tick, set laser AimingVisuals = mLaserAimDir;
        mAimingVisuals.transform.rotation = mLaserAimDir;

        //if we can shoot, but didnt, then disable lr visuals
        if (did_shoot == false)
        {
            lr.enabled = false;
            psEffect.GetComponent<ParticleSystem>().Stop();

        }
        //reset bool
        did_shoot = false;
    }

    override public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        RaycastHit Hit;
        Vector3 bufferdist = ship.transform.position + aimDir * 5f;
        if ( Physics.Raycast(bufferdist, aimDir, out Hit, BeamRange, LayerMask.GetMask("Default")))
        {   //an object was hit
           
            lr.enabled=true;
            lr.SetPosition(0, bufferdist);
            lr.SetPosition(1, Hit.point);

            BeamHitTarget(Hit.collider);

            //Hit Particle effect is a do-not-delete kinda item
            // place particle at Hit.Point and rotate then enable
            psEffect.transform.SetPositionAndRotation(Hit.point, Quaternion.LookRotation(Hit.normal, Vector3.up));
            if (!psEffect.GetComponent<ParticleSystem>().isPlaying)
                psEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            lr.enabled = true;
            lr.SetPosition(0, bufferdist);
            lr.SetPosition(1, bufferdist + aimDir.normalized * BeamRange);
            psEffect.GetComponent<ParticleSystem>().Stop();

        }
    }

    //because there are no Projectiles; we must handle hit code here instead
    private void BeamHitTarget(Collider other)
    {
        if ((other != null) && (other.gameObject.tag != ship.tag) && (other.gameObject.GetComponent<Projectile>() == null))
        {//successful collision that wasnt with shooter
            //Debug.Log("other Entity: " + other.gameObject.tag);
            switch (other.gameObject.tag)
            {
                case "PlayerShip":
                    other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(transform.position, finalBulletDamage * Time.deltaTime);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    break;
                case "EnemyShip":
                    other.gameObject.GetComponentInParent<AICore>().TakeDamage(transform.position, finalBulletDamage * Time.deltaTime);
                    applyImpulse(other.GetComponentInParent<Rigidbody>());
                    break;
                case "Asteroid":
                    other.gameObject.GetComponent<Asteroid>().TakeDamage(finalBulletDamage * Time.deltaTime);
                    applyImpulse(other.GetComponent<Rigidbody>());
                    break;
                case "shard":
                    Destroy(other.transform.gameObject);
                    break;
                default:
                    Debug.Log("Unknown entity. " + other.gameObject.tag);

                    break;
            }
        }
    }

    protected virtual void applyImpulse(Rigidbody body)
    {
        //Vector3 direction = transform.position - body.transform.position;
        body.AddForce(transform.forward * ((finalBulletDamage / 2) + (5 / (2 + body.mass))), ForceMode.Force);
    }

    //called from ship
    override public void Shoot(Vector3 direction)
    {
        //while trying to shoot
        //turn laser sight towards player aim direction.
        Quaternion PlayerShootDir = Quaternion.LookRotation(direction, Vector3.up);
        mLaserAimDir = Quaternion.RotateTowards(mLaserAimDir, PlayerShootDir, mAimingSpeed);

        //test new angle
        float aimDiff = Quaternion.Angle(mLaserAimDir, PlayerShootDir);

        if (aimDiff < 1f)//test angle
        {//if it's close enough, Fire ze Lazers!
            spawnProjectile(direction);
            shootSound.Play();
            //play shooting effect


            did_shoot = true;
        }
        //when reloading
        //  play charge effect (particle effect)

    }

}
