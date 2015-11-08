using UnityEngine;
using System.Collections;
using System.Collections.Generic;      
using Random = UnityEngine.Random; 

public class mainGridSript : MonoBehaviour {

	//vars to assign and create objects for grid.
	public GameObject wallTile;
	public GameObject randomWallTile;
	public GameObject floorTile;
	public GameObject randomPickup;
	public GameObject randomEnemy;
	public GameObject exitNode;
	public int randomObjectCount;
	public int randomWallCount;
	public int randomEnemyCount;

	//vars to handle board and all its information
	public int gridRows;
	public int gridColumns;
	private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.
	public GameObject [,] gridMAP;

	//level Information
	public int currentLevel;
	public int levelSizeInc;
	public int levelItemInc;
	public int levelEnemyInc;

	// Use this for initialization
	void Start () {
		//sets all the available positions in our list of grid positions
		gridPositions.Clear ();
		for (int x=1; x<gridColumns-1; x++) {
			for (int y=1; x<gridRows-1; x++) {
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
		gridMAP = new GameObject[gridRows,gridColumns];
		boardSetup ();
		itemSetup ();
		wallSetup ();
		enemySetup ();
		exitSetup ();
		//AstarPath.active.Scan();

	}


	// Update is called once per frame
	void Update () {
	
	}


	//handles set up of board.
	private void boardSetup(){
		//GameBoardHolder = new GameObject ("Board").transform;
		for (int x = 0; x<gridColumns; x++) {
			for (int y = 0; y<gridRows; y++) {
				GameObject AddTile;
				if (x % 2 == 1 && y % 2 == 1) {
					AddTile = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;

				} else {
					AddTile = Instantiate (floorTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				}
				AddTile.transform.SetParent (this.transform);
				AddTile.name = "tile " + x + "," + y;
				gridMAP[x,y] = AddTile;
			}

			for (int y = -3; y <0; y++) {
					GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
					AddOutside.transform.SetParent (this.transform);
					AddOutside.name = "outside " + x + "," + y;
				}
			for (int y = gridRows; y <gridRows+3; y++) {
					GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
					AddOutside.transform.SetParent (this.transform);
					AddOutside.name = "outside " + x + "," + y;
				}
		}

		for (int y = -3; y <gridRows+3; y++) {
			for (int x = -1; x>-4; x--) {
				GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
				AddOutside.transform.SetParent (this.transform);
				AddOutside.name = "outside " + x + "," + y;
			}
			for (int x = gridColumns; x<gridColumns+3; x++) {
				GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
				AddOutside.transform.SetParent (this.transform);
				AddOutside.name = "outside " + x + "," + y;
			}
		}
	}

	//handles creation of any item pickups
	private void itemSetup(){
		for (int x=0; x<randomObjectCount; x++){
			int lolx;
			int loly;
			while(true){
				loly = Random.Range(2, gridRows-2);
				lolx = Random.Range(2, gridColumns-2);
				if(gridMAP[lolx,loly].tag != "Walls")
					break;
			}
			GameObject RandObj = Instantiate(randomPickup, new Vector3 (lolx, loly, -1f), Quaternion.identity) as GameObject;
			RandObj.transform.SetParent (this.transform);
		}
	}

	//random generation of walls
	private void wallSetup(){
		for (int x=0; x<randomWallCount; x++){
			int lolx;
			int loly;
			while(true){
				loly = Random.Range(2, gridRows-2);
				lolx = Random.Range(2, gridColumns-2);
				if(gridMAP[lolx,loly].tag != "Walls")
					break;
			}
			GameObject RandWall = Instantiate(randomWallTile, new Vector3 (lolx, loly, -1f), Quaternion.identity) as GameObject;
			RandWall.transform.SetParent (this.transform);

			gridMAP[lolx,loly] = RandWall;
		}
	}
	//random generation of walls
	private void enemySetup(){
		for (int x=0; x<randomEnemyCount; x++){
			int lolx;
			int loly;
			while(true){
				loly = Random.Range(4, gridRows-1);
				lolx = Random.Range(4, gridColumns-1);
				if(gridMAP[lolx,loly].tag != "Walls")
					break;
			}
			GameObject RandEnemy = Instantiate(randomEnemy, new Vector3 (lolx, loly, -1f), Quaternion.identity) as GameObject;
			RandEnemy.GetComponent<dumbEnemy> ().gameBoard = this.gameObject;
			RandEnemy.transform.SetParent (this.transform);

		}
	}

	//handles creation of any item pickups
	private void exitSetup(){
		int lolx;
		int loly;
		while(true){
			loly = Random.Range(gridRows/2, gridRows);
			lolx = Random.Range(gridColumns/2, gridColumns);
			if(gridMAP[lolx,loly].tag != "Walls")
				break;
		}
		GameObject randExit = Instantiate(exitNode, new Vector3 (lolx, loly, -1f), Quaternion.identity) as GameObject;
		randExit.transform.SetParent (this.transform);

	}

	public void levelIncrement(){
		currentLevel += 1;
		gridRows += levelSizeInc;
		gridColumns += levelSizeInc;
		randomObjectCount += levelItemInc;
		randomEnemyCount += levelEnemyInc;
		randomWallCount += levelSizeInc;
		resetBoard ();
	}

	private void resetBoard(){
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		Start ();
	}
}
