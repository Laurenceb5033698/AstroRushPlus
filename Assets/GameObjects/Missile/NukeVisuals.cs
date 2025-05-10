using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NukeVisuals : MonoBehaviour {

    private Animator animator;
    private Projectile projectile;
    
	void Start () {
        animator = GetComponentInChildren<Animator>();
        projectile = GetComponent<Projectile>();
        animator.Play("NukeFlash",0);
        
    }
	
	void Update () {
        if (!animator.GetBool("Red") && projectile.m_Stats.lifetime < 2)
        {
            animator.SetBool("Red", true);   
        }

    }
}
