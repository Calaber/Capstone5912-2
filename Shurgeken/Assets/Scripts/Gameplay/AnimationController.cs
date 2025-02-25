﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    DataController data;
    Animator animator;
    int last_animation_id = 0;
	// Use this for initialization
	void Start ()
    {
        data = GetComponent<DataController>();
        animator = GetComponent<Animator>();	
	}

    public string[] animation_state_names = {"Idle","Run Forward","Run Backwards","Run Left","Run Right","Crouch","Jumping","Falling","Landing", "Damaged", "Death","Attack"};	
	// Update is called once per frame
	void LateUpdate () {
        if (data.animation_id != last_animation_id) {
            animator.Play(animation_state_names[data.animation_id]);
            last_animation_id = data.animation_id;
            

            //playing audio for animations
            AudioManager audioManager = GetComponent<AudioManager>();
            audioManager.PlaySound(data.animation_id);
        }
	}
}
