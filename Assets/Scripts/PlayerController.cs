﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private CharacterController controller;
	private Vector3 direction;
	public float forwardSpeed;
	public float maxSpeed;


	private int desiredLane = 1;
	public float laneDistance = 4;

	public float jumpForce;
	public float Gravity = -20; 


	public bool isGrounded;
	public LayerMask groundLayer;
	public Transform groundCheck;

	public Animator animator;
	private bool isSliding = false;




	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerManager.isGameStarted)
			return;
		if(forwardSpeed < maxSpeed)
			forwardSpeed += 0.1f * Time.deltaTime; 


		animator.SetBool ("isGameStarted", true);
		direction.z = forwardSpeed;

		isGrounded = Physics.CheckSphere (groundCheck.position, 0.15f, groundLayer);
		animator.SetBool ("isGrounded", isGrounded);

		if (controller.isGrounded) {

			direction.y = -2 ;
			if (SwipManaager.swipeUp) {
				Jump ();
			}
		}
		else {
			direction.y += Gravity * Time.deltaTime;
		}


		if (SwipManaager.swipeDown && !isSliding) {

			StartCoroutine (Slide ());
		}


		if (SwipManaager.swipeRight)  {
			desiredLane++;
			if (desiredLane == 3)
				desiredLane = 2;
		}

		if (SwipManaager.swipeLeft) {
			desiredLane--;
			if (desiredLane == -1)
				desiredLane = 0;
		}

		Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

		if (desiredLane == 0) {
			targetPosition += Vector3.left * laneDistance;
		} else if (desiredLane == 2) {
			targetPosition += Vector3.right * laneDistance;
		}

		if (transform.position == targetPosition)
			return;
		
		Vector3 diff = targetPosition - transform.position;
		Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
		if (moveDir.sqrMagnitude < diff.magnitude)
			controller.Move(moveDir);
		else
			controller.Move(diff);
		
	}


	private void FixedUpdate()
	{
		if (!PlayerManager.isGameStarted)
			return;
		
		controller.Move (direction * Time.deltaTime);

	}

	private void Jump()
	{
		direction.y = jumpForce;

	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.transform.tag == "Obstacle")
		{
			PlayerManager.gameOver = true;
			FindObjectOfType<AudioManager> ().PlaySound ("GameOver");
		}
	}


	private IEnumerator Slide (){

		isSliding = true;
		animator.SetBool ("isSliding", true);
		controller.center = new Vector3 (0, -0.5f , 0);
		controller.height = 1;

		yield return new WaitForSeconds (1.3f);

		controller.center = new Vector3 (0, 0 , 0);
		controller.height = 2;


		animator.SetBool ("isSliding", false);
		isSliding = false;
	}
}
