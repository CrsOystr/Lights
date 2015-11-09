using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class charController : MonoBehaviour {

	//camera attatched to player
	public GameObject sceneCamera;

	//movementt and location information about player
	public float moveSpeed;
	private Vector3 moveDirection = Vector3.right;
	public Vector3 pos;                               

	//vars that determine the players light source
	public GameObject sceneLight;
	public float lightLoss;
	public float lightAngle = 60;
	public float lightLevel = 6.6f;

	//gives character information about board to ensure movement is correct
	public GameObject gameBoard;
	public GameObject playerMesh;
	public mainGridSript mainGrid;
	private int gameRows;
	private int gameColumns;

	//level initialize timer variables
	public float startTime;
	private float startTimer;
	private bool started = false;

	//score variables
	public int stepsTaken;
	public GameObject scoreBoard;
	public GameObject gameOver;

	//control variablea
	private float inputHorz;
	private float inputVert;


	// Use this for initialization
	void Start () {
		//sets initial starting location
		this.transform.position = new Vector3 (0f, 0f, -1);
		pos = transform.position;

		//gets information about the board he is located on
		mainGrid = gameBoard.GetComponent<mainGridSript>();
		gameRows = mainGrid.gridRows;
		gameColumns = mainGrid.gridColumns;
		startTimer = Time.time;
		stepsTaken = 0;
	}
	
	// Update is called once per frame
	void Update () {
		rotateCharacter();
		if (started) {
			moveCharacter ();
			lightUpdate ();
		}
		//this adds a delay so game doesnt start right 
		else if ((Time.time - startTimer) > startTime)
		{
			started = true;
		}
	}

	// increases light 
	public void batteryPickup(){
		this.sceneLight.GetComponent<Light>().intensity += 2.4f;
		}

	//activates every second to reduce player light resource
	private void lightUpdate(){
		sceneLight.GetComponent<Light>().intensity -= Time.deltaTime * lightLoss + Random.Range (-1f, 1f)/14;
		sceneLight.GetComponent<Light>().spotAngle = 30 + (sceneLight.GetComponent<Light>().intensity * 3);// + Random.Range (0, 2);
		if (sceneLight.GetComponent<Light>().intensity < .2) {
			killCharacter();
			}
		}

	private void rotateCharacter(){
		if (moveDirection == Vector3.right) {
			playerMesh.transform.localEulerAngles = new Vector3 (0, 90, 270);
		} else if (moveDirection == Vector3.up) {
			playerMesh.transform.localEulerAngles = new Vector3 (270, 90, 270);
		} else if (moveDirection == Vector3.down) {
			playerMesh.transform.localEulerAngles = new Vector3 (90, 90, 270);
		} else if (moveDirection == Vector3.left) {
			playerMesh.transform.localEulerAngles = new Vector3 (180, 90, 270);
		}
	}

	//extremely messy character movement
	private void moveCharacter(){
		if (Input.GetAxisRaw ("Horizontal") != 0)
			inputHorz = Input.GetAxisRaw ("Horizontal");
		
		if (Input.GetAxisRaw ("Vertical") != 0)
			inputVert = Input.GetAxisRaw ("Vertical");

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
			if (Mathf.Abs (touchDeltaPosition.x) > Mathf.Abs (touchDeltaPosition.y)) {
				if (touchDeltaPosition.x > 0) {
					inputHorz = 1;
					inputVert = 0;
				} else {
					inputHorz = -1;
					inputVert = 0;
				}
			} else {
				if (touchDeltaPosition.y > 0) {
					inputVert = 1;
					inputHorz = 0;
				} else {
					inputVert = -1;
					inputHorz = 0;
				}
			}
			Debug.Log ("X, Y: " + touchDeltaPosition.x + ", " + touchDeltaPosition.y);
		}
		
		
		if ((int)(pos.x + inputHorz) < 0 || (int)(pos.x + inputHorz) > gameColumns - 1) {
			inputHorz = 0;
		}
		if ((int)(pos.y + inputVert) < 0 || (int)(pos.y + inputVert) > gameRows - 1) {
			inputVert = 0;
		}
		
		//for jeff
		if (transform.position.x % 1 == 0 || (transform.position.y % 1 == 0)){
			Debug.Log(inputHorz);
			if ((pos.x < 0 || pos.x > gameColumns - 1) ) {
				moveDirection = -moveDirection;
				pos += moveDirection;
				moveDirection = Vector3.zero;
				inputHorz = inputVert = 0;
			} else if (pos.y < 0 || pos.y > gameRows - 1) {
				moveDirection = -moveDirection;
				pos += moveDirection;
				moveDirection = Vector3.zero;
				inputHorz = inputVert = 0;
			} else if (mainGrid.gridMAP [(int)pos.x, (int)pos.y].tag == "Walls") {
				moveDirection = -moveDirection;
				pos += moveDirection;
				moveDirection = Vector3.zero;
				Debug.Log("WALL");
				Debug.Log(pos);
				inputHorz = inputVert = 0;
			} else if (transform.position == pos && inputHorz != 0 && mainGrid.gridMAP [(int)(pos.x + inputHorz), (int)pos.y].tag != "Walls") {
				pos += Vector3.right * inputHorz;	
				moveDirection = Vector3.right * inputHorz;
				inputHorz = inputVert = 0;
				Debug.Log("HOR");
				Debug.Log(pos);
				stepsTaken++;
			} else if (transform.position == pos && inputVert != 0 && mainGrid.gridMAP [(int)(pos.x), (int)(pos.y + inputVert)].tag != "Walls") {
				pos += Vector3.up * inputVert;
				moveDirection = Vector3.up * inputVert;
				inputHorz = inputVert = 0;
				Debug.Log("VERT");
				Debug.Log(pos);
				stepsTaken++;
			} else if (transform.position == pos) {
				pos += moveDirection;
				if (Mathf.Abs(moveDirection.x) > 0 || Mathf.Abs(moveDirection.y)>0){
					stepsTaken++;
				}

			}
			scoreBoard.GetComponent<Text>().text  = "Steps Taken: " + stepsTaken + " Levels Escaped: " + mainGrid.currentLevel;
		}
		transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * moveSpeed);
	}

	//resets character
	public void levelWin(){
		gameBoard.GetComponent<mainGridSript> ().levelIncrement ();
		resetCharacter ();

	}

	//resets character
	public void resetCharacter(){
		//sets initial starting location
		this.transform.position = new Vector3 (0f, 0f, -1);
		pos = transform.position;
		moveDirection = Vector3.right;
		gameRows = mainGrid.gridRows;
		gameColumns = mainGrid.gridColumns;
		startTimer = Time.time;
	}

	//kills  character dead
	public void killCharacter(){
		moveSpeed = 0;

		gameOver.SetActive (true);
		//Application.LoadLevel ("Title");
	}

}
