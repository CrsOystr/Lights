using UnityEngine;
using System.Collections;
using Pathfinding;

public class enemyScript : MonoBehaviour {

	//The point to move to
	public Vector3 targetPosition;
	private Seeker seeker;
	private CharacterController controller;
	//The calculated path
	public Path path;
	//The AI's speed per second
	public float speed = 1f;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 1;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	public void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}

	public void OnPathComplete (Path p) {
		Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	public void Update () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			return;
		}
		//Direction to the next waypoint
		Vector3 newDir = (path.vectorPath [currentWaypoint] - transform.position);
		if (newDir.x > newDir.y)
		{
			newDir = new Vector3(newDir.x,0,0);
		}
		else
		{
			newDir =new Vector3(0,newDir.y,0);
		}
		Vector3 dir = newDir.normalized;
		dir *= speed * Time.deltaTime;
		//controller.Move (dir);
		this.gameObject.transform.Translate( dir );
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
}
