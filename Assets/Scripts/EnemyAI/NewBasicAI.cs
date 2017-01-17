using UnityEngine;
using System.Collections;

public class NewBasicAI : MonoBehaviour {

    [SerializeField] private GameObject ship;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 destination;
    [SerializeField] private int state = 0;
    [SerializeField] private GameObject sceneManager;


    private EnemyStats stats;

    [SerializeField]
    private GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
