using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gatling : Weapon_Cannon
{
    public float Max_Firerate= 10.0f; //shots per second
    public float Min_Firerate = 1.0f; //shots per second at start of Ramp. 0 = 1 shot/sec


    private float Current_Ramp = 0f;//percentage of max firerate. range from 0-ramp_up_period
    public float Ramp_up_Period = 4.0f; // time spent winding up in seconds

    private float Current_Burnout = 0.0f;
    public float Burnout_Period = 5.0f; // How long weapon can spend firing continuously in seconds
    public float CoolDown_Period = 2.0f;// How long it takes to completely cool off before firing again in seconds
    private bool cooling_off = false;
    private bool did_Shoot = false;

    private void OnEnable()
    {   //from Weapon_Cannon
        mLaserAimDir = transform.rotation;
        mAimingVisuals.SetActive(true);
        mAimingVisuals.transform.rotation = mLaserAimDir;
    }

    private void OnDisable()
    {   //from Weapon_Cannon
        mAimingVisuals.SetActive(false);
    }

    void Update()
    {   //from Weapon_Cannon
        mAimingVisuals.transform.rotation = mLaserAimDir;

        if (cooling_off)
        {
            Current_Burnout -= Time.deltaTime;
            if (Current_Burnout <= 0f)
            {
                cooling_off = false;
                Current_Burnout = 0f;
                Current_Ramp = 0f;
            }
        }
        else
        if (did_Shoot)
        {//
            if (Current_Ramp < Ramp_up_Period)
            {
                Current_Ramp += Time.deltaTime;//increase firerate
            }
            else{
                Current_Burnout += Time.deltaTime;
            }

            if (Current_Burnout >= Burnout_Period){
                cooling_off = true;
                Current_Burnout = CoolDown_Period;
            }
        }
        else//if you can shoot, but didnt
        {//decrease firerate and burnout
            if (Current_Ramp > 0f)
                Current_Ramp -= Time.deltaTime;
            if (Current_Burnout > 0f)
                Current_Burnout -= Time.deltaTime;
            
        }
        did_Shoot = false;
    }

    override public void spawnProjectile(Vector3 aimDir)
    {//spawn pattern for weapon type
        aimDir += (new Vector3(Random.Range(-Current_Burnout, Current_Burnout), 0, Random.Range(-Current_Burnout, Current_Burnout))/32);
        GameObject mBullet;
        mBullet = (GameObject)Instantiate(bullet1, ship.transform.position + aimDir * 6f, Quaternion.LookRotation(aimDir, Vector3.up));
        mBullet.GetComponent<Projectile>().SetupValues(finalBulletDamage, bulletSpeed, ship.tag);

    }

    override public void Shoot(Vector3 direction)
    {
        //while trying to shoot
        //turn laser sight towards player aim direction.
        Quaternion PlayerShootDir = Quaternion.LookRotation(direction, Vector3.up);
        mLaserAimDir = Quaternion.RotateTowards(mLaserAimDir, PlayerShootDir, mAimingSpeed);

        //test new angle
        float aimDiff = Quaternion.Angle(mLaserAimDir, PlayerShootDir);

        if (!cooling_off)
        {
            if (Time.time > reload)
            {
                if (aimDiff < 5f)//test angle
                {//if it's close enough, shoot bullet
                    shootSpeed = (1 / (1 + Min_Firerate + Max_Firerate * (Current_Ramp / Ramp_up_Period)));
                    reload = Time.time + shootSpeed;
                    spawnProjectile(direction);

                    shootSound.Play();

                }
            }
        did_Shoot = true;
        }
    }
}
