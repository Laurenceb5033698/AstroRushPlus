using UnityEngine;
using System.Collections;

abstract public class Weapon : MonoBehaviour{
    /*         !!! ABSTRACT CLASS !!!
     *  Don't actually attach this to anything!
     */
    [SerializeField] protected GameObject bullet1;//bullet prefab
    [SerializeField] protected GameObject TurretPref;//bullet prefab


    [SerializeField] protected GameObject ship; //refernece to ship
    //protected GameObject turret = null;
    [SerializeField] protected float shootSpeed;//fiddle with this one
    
    protected float reload = 0;

    [SerializeField] protected float bulletDamage = 5f;
    [SerializeField] protected float bulletSpeed = 20f;
    [SerializeField] protected AudioSource shootSound;

    virtual public void Start () // Use this for initialization
    {
        shootSound = GetComponent<AudioSource>();
        

    }
    public void volumeChanged(float val)
    {
        shootSound.volume = val;
    }
    private void OnEnable()
    {
        if (TurretPref != null)
        {
            GetComponentInParent<Arsenal>().SwapTurret(TurretPref);

        }

    }

    private void OnDisable()
    { }
    

    void Update () // Update is called once per frame
    {	}

    public void SetShipObject(GameObject obj)
    {
        ship = obj;
        reload = 0;
    }
    abstract public void spawnProjectile(Vector3 aimDir);

    abstract public void Shoot(Vector3 direction);

}
