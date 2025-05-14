using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public struct BulletStats
{
    public float damage;
    public float speed;
    public float acceleration;
    public float range;
    public float lifetime;
    public float magnet;
    public int   penetration;
    public int   riccochet;
    public float size;
    public float falloff;
    //other bools might be needed here.

    //ctor from stats.
    public BulletStats(Stats _stats)
    {
        damage = _stats.Get(StatType.gAttack);
        speed = _stats.Get(StatType.bSpeed);
        acceleration = _stats.Get(StatType.bAcceleration);
        range = _stats.Get(StatType.bRange);
        lifetime = _stats.Get(StatType.bLifetime);
        magnet = _stats.Get(StatType.bMagentPower);
        penetration = (int)_stats.Get(StatType.bPenetrationAmount);
        riccochet = (int)_stats.Get(StatType.bRicochetAmount);
        size = _stats.Get(StatType.bSize);
        falloff = Mathf.Max(1,(_stats.Get(StatType.bFalloff) * _stats.Get(StatType.bFalloff)));
    }
    public BulletStats(Stats _stats, Rigidbody _rb)
    {   //missile type projectile
        damage = _stats.Get(StatType.mAttack);
        speed = _stats.Get(StatType.mSpeed);
        acceleration = _stats.Get(StatType.mAcceleration);
        range = _stats.Get(StatType.mRange);
        lifetime = _stats.Get(StatType.mLifetime);
        magnet = _stats.Get(StatType.mMagnetPower);
        penetration = 0;
        riccochet = 0;
        size = _stats.Get(StatType.mSize);
        falloff = 1;
    }
}

public class Projectile : MonoBehaviour {

    public string ownertag;
    public GameObject psImpactPrefab;

    public BulletStats m_Stats;

    //values for range and falloff
    Vector3 m_PreviousPos;
    float m_sqrCumulativePath = 0.0f;
    float m_falloffCurrent = 0.0f;
    float m_falloffPercent = 1.0f;
    float m_sizeModifier = 1.0f;

    void Start () {
        //begin recording path for range.
        m_PreviousPos = transform.position;
	}
    virtual public void SetupValues(string _ownerTag, Stats _setupStats)
    {
        ownertag = _ownerTag;
        m_Stats = new BulletStats(_setupStats);
    }
    protected virtual void OnTriggerEnter(Collider _other)
    {

        if ((_other != null) && (_other.gameObject.tag != ownertag) && (!_other.gameObject.CompareTag("bullet")))
        {//successful collision that wasnt with shooter
            //Debug.Log("other Entity: " + other.gameObject.tag);
            bool hit = false;
            Damageable otherDamagable = _other.GetComponent<Damageable>();
            if (otherDamagable)
            {
                otherDamagable.TakeDamage(transform.position, CalcDamage());
                applyImpulse(otherDamagable.GetRigidbody());
                hit = true;
            }

            //case: other is not damageable but can be hit anyway?
            //Debug.Log("Unknown entity. " + other.gameObject.tag);
            
            if (hit)
            {
                //reduce riccochet first
                if(m_Stats.riccochet > 0)
                { 
                    m_Stats.riccochet--;
                    SpawnHitVisuals();  //hit visuals for each rccochet
                    
                }
                else
                {
                    if(m_Stats.penetration > 0)
                    {
                        m_Stats.penetration--;
                        SpawnHitVisuals();
                    }
                    else
                    {
                        //out of riccochet and pen, queue destroy.
                        m_Stats.lifetime = -1;
                    }
                }
            }
        }
    }

    protected virtual void applyImpulse(Rigidbody body)
    {
        //Vector3 direction = transform.position - body.transform.position;
        body.AddForce(transform.forward * ((m_Stats.damage / 2)+(m_Stats.speed / (2+body.mass))), ForceMode.Impulse);
    }

    // Update is called once per frame
    protected virtual void Update () {

        transform.position += transform.forward * CalcSpeed()* Time.deltaTime;
        m_Stats.lifetime -= Time.deltaTime;
        if (m_Stats.lifetime < 0)
            DestroySelf();
	}

    private void FixedUpdate()
    {
        //check these in fixed update to reduce lag on higher framerates.
        CheckRange();
        BulletFalloff();
    }

    protected virtual void DestroySelf()
    {// perhaps spawn a particle? like missile does
        SpawnHitVisuals();
        Destroy(transform.gameObject);
    }

    protected virtual void SpawnHitVisuals()
    {
        Instantiate(psImpactPrefab, transform.position, transform.rotation);

    }

    private void CheckRange()
    {
        //bullets have an effective range.
        //it can travel upto that range as fast as it likes, then must falloff
        //range can be reached independantly from lifetime.
        float segmentSqrMag = (transform.position - m_PreviousPos).sqrMagnitude;
        m_sqrCumulativePath += segmentSqrMag;
        if(m_sqrCumulativePath > (m_Stats.range * m_Stats.range))
        {
            //add current overshoot to falloff
            m_falloffCurrent += segmentSqrMag;
            //percent scales from 1 -> 0 as falloffcurrent increases.
            m_falloffPercent = Mathf.Clamp((1 - ( m_falloffCurrent / m_Stats.falloff)),0,1);
        }
        
    }
    private void BulletFalloff()
    {
        if (m_falloffCurrent > m_Stats.falloff)
        {
            //current falloff is over max falloff. queue deletion.
            m_Stats.lifetime = -1;
            return;
        }
        //when bullet falloff increases, it reduces bullet size, speed and damage
        float uniformScale = m_Stats.size* m_falloffPercent;
        transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);

    }

    virtual protected float CalcDamage()
    {
        //falloff percent = 1 when falloffcurrent = 0
        float damageModified = m_Stats.damage * m_falloffPercent;
        return damageModified;
    }

    virtual protected float CalcSpeed()
    {
        //falloff percent = 1 when falloffcurrent = 0
        float speedModified = m_Stats.speed * m_falloffPercent;
        return speedModified;
    }

}
