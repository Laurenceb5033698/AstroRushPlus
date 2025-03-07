using UnityEngine;

public class Universal_Weapon : Universal_Weapon_Base
{
    //reads values straight from ship's stats
    //is set from owner controller

    

    private void Awake()
    {
        
    }


    void Start()
    {
        
    }
    
    //for visual updates
    void Update()
    {
        
    }
    //for gameplay updates
    private void FixedUpdate()
    {
        //reduce attack interval
        
        //cooldown
        
        //reload

        //charge
    }


    protected override void Shoot()
    {
        //check shooting conditions
        //m_AttackInterval
        

    }

    //when shoot is successful, does all spawning of projectiles
    protected override void SpawnProjectilesImpl()
    {
        
    }



}
