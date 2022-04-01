using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (60 * Time.deltaTime, 0, 0);  
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			FindObjectOfType<AudioManager> ().PlaySound ("Pickup");
			PlayerManager.numberOfCoins += 1;	

			Destroy (gameObject);
		}

	}

}
