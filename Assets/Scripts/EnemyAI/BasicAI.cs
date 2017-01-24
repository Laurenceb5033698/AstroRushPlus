using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

    private GameObject player;
    private GameObject sm;
    private Rigidbody rb;
    private int id;

    private float speed;

	// Use this for initialization
	void Start () 
    {
        rb = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        // transform.position + transform.TransformDirection(new Vector3(controls.rightY, controls.yawAxis, 0)) * stats.GetLaserRange();
        speed = 15 * Time.deltaTime;
        transform.LookAt(player.transform.position);
        //rb.velocity = (player.transform.position - transform.position) * speed;

        rb.AddForce((player.transform.position - transform.position) * speed, ForceMode.Force);
    }

    public void Initalise(GameObject go, GameObject s, int i)
    {
        player = go;
        sm = s;
        id = i;
    }

    void OnCollisionEnter(Collision c)
    {
        sm.GetComponent<EnemyManager>().RemoveShip(id);
        DestroySelf();
    }

    private void DestroySelf()
    {
        Debug.Log("ship destroyed");
        Destroy(transform.gameObject);
    }

}
