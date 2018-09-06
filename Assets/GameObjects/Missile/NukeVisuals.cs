using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NukeVisuals : MonoBehaviour {

    private Animator animator;
    private Animation manim;
    private Projectile projectile;
    bool playing = false;


	void Start () {
        animator = GetComponentInChildren<Animator>();
        manim = GetComponentInChildren<Animation>();
        projectile = GetComponent<Projectile>();
        //animator.Play("SignFlashing");
        //animator.Play("SignFlashing", 0);
        //manim.Play("SignFlashing");
    }
	
	void Update () {
        if (!playing && projectile.lifetime < 2)
        {
            //animator.Play("RedBlend",1);
            manim.Play("RedBlend");
            playing = true;
        }

    }
}
