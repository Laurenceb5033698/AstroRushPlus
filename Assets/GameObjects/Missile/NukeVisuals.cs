using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NukeVisuals : MonoBehaviour {

    private Animator animator;
    private Projectile projectile;
    bool playing = false;


	void Start () {
        animator = GetComponentInChildren<Animator>();
        projectile = GetComponent<Projectile>();
        //animator.Play("SignFlash");
    }
	
	void Update () {
        if (!playing && projectile.lifetime < 2)
            animator.Play("RedBlend");
	}
}
