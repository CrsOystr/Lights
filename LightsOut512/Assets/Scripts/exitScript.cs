using UnityEngine;
using System.Collections;

public class exitScript : MonoBehaviour {

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
			other.GetComponent<charController>().levelWin();
		}
	}
}
