using UnityEngine;
using UnityEngine.VFX;


/// <summary>
/// refactored for new shield material.
/// </summary>
public class Shield : MonoBehaviour {

    private Stats stats;
    private MeshRenderer mr;

    //public float hitFlashDuration = 0.3f;
    private VisualEffect vfxShield;
    private Animator controller;

    private bool ShieldUp;

    public bool ShieldState 
    { 
        get 
        {   return ShieldUp; }
        set 
        { 
            ShieldUp = value; 
            SetShieldState(); 
        } 
    }

    private void Awake()
    {
        controller = GetComponent<Animator>();
        vfxShield = GetComponent<VisualEffect>();
    }

    void Start () 
    {
        
	    stats = transform.gameObject.GetComponentInParent<Stats>();
        ShieldState = false;
	}


    void Update () 
    {
        //if shield state changes, then tell animation to transition.
        bool statShield = stats.IsShielded();
        if (statShield != ShieldState)
        {
            ShieldState = statShield; 
        }

        //mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, 0.2f * (stats.ShipShield / stats.GetShieldMax()));
        ////mr.materials[0].color = new Color(mr.materials[0].color.r, mr.materials[0].color.g, mr.materials[0].color.b, (strength/100));
        //mr.enabled = (stats.ShipShield < 0.1f) ? false : true;
    }

    //newshield is a vfxgrpah object, with exposed properties for controlling an instance of the shield material

    public void SetShieldState()
    {
        controller.SetBool("ShieldHasHealth", ShieldUp);
    }

    public void PowerUp()
    {
        controller.SetBool("ShieldHasHealth", true);
    }

    public void PowerDown()
    {
        controller.SetBool("ShieldHasHealth", false);
    }

    public void ShieldHit(Vector3 _pos)
    {
        if (ShieldUp)
        {
            Vector3 hitLocal = _pos - transform.position;
            Vector3 rotatedHit = Quaternion.Inverse(transform.rotation) * hitLocal;
            float hitFlashDuration = vfxShield.GetFloat("HitFlashDuration");
            vfxShield.SetVector3("HitPosition", rotatedHit);
            vfxShield.SetFloat("HitTime", Time.time + hitFlashDuration);
        }
    }


}
