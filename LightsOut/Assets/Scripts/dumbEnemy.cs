using UnityEngine;
using System.Collections;

public class dumbEnemy : MonoBehaviour {
	
	//movementt and location information about player
	public float moveSpeed;
	private Vector3 moveDirection;
	public Vector3 pos;  

	//gives character information about board to ensure movement is correct
	public GameObject gameBoard;
	private mainGridSript mainGrid;
	private int gameRows;
	private int gameColumns;
	
	// Use this for initialization
	void Start () {
		pos = transform.position;

		//gets information about the board he is located on
		mainGrid = gameBoard.GetComponent<mainGridSript>();
		gameRows = mainGrid.gridRows;
		gameColumns = mainGrid.gridColumns;
	}
	
	// Update is called once per frame
	void Update () {
		moveEnemy ();
		if (moveDirection == Vector3.right) {
			transform.localEulerAngles = new Vector3(0,90,270);
		}else if(moveDirection == Vector3.up){
			transform.localEulerAngles = new Vector3(270,90,270);
		}else if(moveDirection == Vector3.down){
			transform.localEulerAngles = new Vector3(90,90,270);
		}else{
			transform.localEulerAngles = new Vector3(180,90,270);
		}
	}


	//extremely messy character movement
	private void moveEnemy(){	
		int inputHorz = Random.Range (-1, 2);
		int inputVert = Random.Range (-1, 2);
		if((int)(pos.x+inputHorz) < 0 ||(int)(pos.x+inputHorz) > gameColumns-1){
			inputHorz = 0;
		}
		if((int)(pos.y+inputVert) < 0 ||(int)(pos.y+inputVert) > gameRows-1){
			inputVert= 0;
		}
		if(pos.x<0 || pos.x>gameColumns-1){
			moveDirection = -moveDirection;
			pos +=moveDirection * 2;
		} else if(pos.y<0 || pos.y>gameRows-1){
			moveDirection = -moveDirection;
			pos +=moveDirection * 2;
		} else if(mainGrid.gridMAP[(int)pos.x,(int)pos.y].tag == "Walls"){
			moveDirection = -moveDirection;
			pos +=moveDirection * 2;
		}else if (transform.position == pos && inputHorz != 0 && mainGrid.gridMAP[(int)(pos.x+inputHorz),(int)pos.y].tag != "Walls"){
			if (moveDirection != Vector3.right && moveDirection != Vector3.left)
			{
				pos += Vector3.right*inputHorz;	
				moveDirection = Vector3.right*inputHorz;
			}
		}
		else if (transform.position == pos && inputVert != 0 && mainGrid.gridMAP[(int)(pos.x),(int)(pos.y+inputVert)].tag != "Walls") {
			if (moveDirection != Vector3.up && moveDirection != Vector3.down)
			{
				pos += Vector3.up*inputVert;
				moveDirection = Vector3.up*inputVert;
			}
		}else if (transform.position == pos){
			pos +=moveDirection;
		}
		
		
		transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * moveSpeed);
	}

	void randomDirection()
	{
		int rand = Random.Range (0, 4);
		switch (rand) {
		case 0:
				moveDirection = Vector3.left;
				break;
		case 1:
				moveDirection = Vector3.right;
				break;
		case 2:
				moveDirection = Vector3.up;
				break;
		case 3:
				moveDirection = Vector3.down;
				break;
		}
	}


	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			Destroy (this.gameObject);
			other.GetComponentInParent<charController> ().killCharacter();
		}
	}
}
	