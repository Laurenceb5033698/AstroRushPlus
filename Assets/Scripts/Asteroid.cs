using UnityEngine;
using System.Collections;


public class Asteroid : MonoBehaviour
{
    private GameObject am; // asteroid manager
    private Health hp;
    [SerializeField] private GameObject psDestructPrefab;

    void OnEnable()
    {
        hp.SetHealth(200);
    }

    void Awake()
    {
        hp = gameObject.AddComponent<Health>();
       // hp = new Health();
    }

    public void SetAsteroidManager(GameObject a)
    {
        am = a;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.magnitude > 5f)
        {
            TakeDamage(c.relativeVelocity.magnitude);
            
        }
    }
    public void TakeDamage(float val)
    {
        hp.TakeDamage(val);
        if (!hp.IsAlive())
        {   //asteroid dies
            //spawn debris and particles
            am.GetComponent<AsteroidManager>().SpawnChunks(transform.position);

            gameObject.SetActive(false);
            Reset();
        }
    }

    public void Reset()
    {   //tell asteroidManager that i wish to be reset
        Instantiate(psDestructPrefab, transform.position, transform.rotation);
        am.GetComponent<AsteroidManager>().Reset(gameObject);
        gameObject.SetActive(true);
    }

}
