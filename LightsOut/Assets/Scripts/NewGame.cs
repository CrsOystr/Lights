using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {

	public float soundTime;
	private float soundTimer;
	public GameObject musicPlayer;

	// Use this for initialization
	void Start () {
		soundTimer = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - soundTimer > soundTime)
		{
			musicPlayer.SetActive(false);
		}
	}

	public void newGame(string level){
		Application.LoadLevel (level);
	}
}
