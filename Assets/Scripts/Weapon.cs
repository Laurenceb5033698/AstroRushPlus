using UnityEngine;
using System.Collections;

abstract public class Weapon : MonoBehaviour{
    /*         !!! ABSTRACT CLASS !!!
     *  Don't actually attach this to anything!
     */
    [SerializeField] protected GameObject bullet1;//bullet prefab
    [SerializeField] protected GameObject TurretPref;//bullet prefab


    [SerializeField] protected GameObject ship; //refernece to ship
    protected GameObject turret = null;
    [SerializeField] protected float shootSpeed;//fiddle with this one
    
    protected float reload = 0;

    [SerializeField] protected float bulletDamage = 5f;
    [SerializeField] protected float bulletSpeed = 20f;
    [SerializeField] protected AudioSource shootSound;

    virtual public void Start () // Use this for initialization
    {
        shootSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if(TurretPref != null)
        {
            float yrot = (Vector3.Angle(Vector3.right, ship.transform.right));
            //Debug.Log("Turret angle: " + yrot);
            if (Vector3.Cross(ship.transform.right, Vector3.right).y < 0) yrot = -yrot;
            
            yrot = yrot * Mathf.PI / 180;
            Vector3 loca = ship.transform.position + new Vector3(0.589f * Mathf.Cos(yrot), 0.651f, 0.589f * Mathf.Sin(yrot));
            turret = Instantiate<GameObject>(TurretPref, loca, Quaternion.LookRotation(Vector3.up, -ship.transform.right), ship.transform);//
        }
    }

    private void OnDisable()
    {
        if (turret)
        {
            Destroy(turret);
            turret = null;
        }
    }

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
