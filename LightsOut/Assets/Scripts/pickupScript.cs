using UnityEngine;
using System.Collections;

public class pickupScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (20 * Vector3.back * Time.deltaTime);
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Destroy (this.gameObject);
			other.GetComponentInParent<charController> ().batteryPickup ();

		}
	}
}
