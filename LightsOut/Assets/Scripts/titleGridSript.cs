using UnityEngine;
using System.Collections;
using System.Collections.Generic;      
using Random = UnityEngine.Random; 

public class titleGridSript : MonoBehaviour {

	//vars to handle size of board and objects to put on board
	public GameObject wallTile;
	public GameObject floorTile;
	public GameObject randomPickup;
	public int randomObjectCount;

	//vars to handle the board
	public int gridRows;
	public int gridColumns;
	private List <Vector3> gridPositions = new List <Vector3> (); 

	//vars to handle title 
	public GameObject letters;
	private Light[] lightArray;
	public float lightTime;
	private float lightTimer;

	// Use this for initialization
	void Start () {
		//initializes lights for title workds
		lightTimer = Time.time;
		lightArray = letters.GetComponentsInChildren<Light>();
		for (int i = 0; i<lightArray.Length; i++) {
			lightArray[i].intensity = 4;
			lightArray[i].spotAngle = 20;
		}

		//initializes the list of grid positoons we can place things
		gridPositions.Clear ();
		for (int x=1; x<gridColumns-1; x++) {
			for (int y=1; x<gridRows-1; x++) {
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}

		boardSetup ();
		itemSetup ();
	}


	// Update is called once per frame
	void Update () {
		titleUpdate ();
	}


	//creates the game board
	private void boardSetup(){
		for (int x = 0; x<gridColumns+2; x++) {
			for (int y = 0; y<gridRows+2; y++) {
				GameObject AddTile;
				if (x % 2 == 1 && y % 2 == 1) {
					AddTile = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;

				} else {
					AddTile = Instantiate (floorTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				}
				AddTile.transform.SetParent (this.transform);
				AddTile.name = "tile " + x + "," + y;
			}

			for (int y = -3; y <0; y++) {
					GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
					AddOutside.transform.SetParent (this.transform);
					AddOutside.name = "outside " + x + "," + y;
				}
			for (int y = gridRows+2; y <gridRows+5; y++) {
					GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
					AddOutside.transform.SetParent (this.transform);
					AddOutside.name = "outside " + x + "," + y;
				}
		}

		for (int y = -3; y <gridRows+5; y++) {
			for (int x = -1; x>-4; x--) {
				GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
				AddOutside.transform.SetParent (this.transform);
				AddOutside.name = "outside " + x + "," + y;
			}
			for (int x = gridColumns+2; x<gridColumns+5; x++) {
				GameObject AddOutside = Instantiate (wallTile, new Vector3 (x, y, -1f), Quaternion.identity) as GameObject;
				AddOutside.transform.SetParent (this.transform);
				AddOutside.name = "outside " + x + "," + y;
			}
		}
	}

	//sets up items, not necessary for title but maybe ill use it
	private void itemSetup(){
		for (int x=0; x<randomObjectCount; x++){
			int lolx;
			int loly;
			while(true){
				loly = Random.Range(2, gridRows-2);
				lolx = Random.Range(2, gridColumns-2);
				if(!(lolx % 2 == 1 && loly % 2 == 1))
					break;
			}
			
			GameObject RandObj = Instantiate(randomPickup, new Vector3 (lolx, loly, -1f), Quaternion.identity) as GameObject;
		}
	}
	
	//Updates the lights for the letters of the title
	private void titleUpdate(){
		if (Time.time - lightTimer < lightTime)
		{
			for (int i = 0; i<lightArray.Length; i++) {
				lightArray [i].intensity += .001f + Time.deltaTime * Random.Range(-5f,15f)/20;     
				lightArray [i].spotAngle += .02f + Time.deltaTime * Random.Range(-13f,15f);
				
			}
		}
		else
		{
			for (int i = 0; i<lightArray.Length; i++) {
				lightArray [i].intensity += Time.deltaTime * Random.Range(-15f,15f)/10;     
				lightArray [i].spotAngle +=Time.deltaTime * Random.Range(-17f,17f);
				
			}
		}
	}

	//loads the level to play
	public void loadGame(){
		Application.LoadLevel ("Nic");
	}
}
