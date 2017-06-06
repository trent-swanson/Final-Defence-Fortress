using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	float playerHealth = 100;

	//rb is a reference to the player's Rigidbody component
	public Rigidbody rb;
	//controller is a reference to a specific controller
	public XboxController controller;

	//Moving
	//moveSpeed is player's base speed
	public float moveSpeed = 60;
	//turnSpeed is player's left and right turning speed
	public float turnSpeed = 10;
	//lookUpSpeed is the player's look up and down speed
	public float lookUpSpeed = 10;

	//Jumping
	//Player's jump velocity between 1 and 10
	[Range(1, 10)]
	public float jumpVelocity = 5;
	//fallMultiplier is how fast falling is sped up
	public float fallMultiplier = 2.5f;
	//lowJumpMultiplier is how fast falling is sped up on smaller jumps
	public float lowJumpMultiplier = 2;

	//Shooting
	//bulletParent is a reference to the bullet parent transform
	public Transform bulletParent;
	//bulletPrefab is a reference to the bullet prefab
	public GameObject bulletPrefab;
	//bulletSpawn is a reference to the bullet spawn transform
	public Transform bulletSpawn;
	//bulletSpeed is how fast the bullet moves
	public float bulletSpeed = 6;
	//shootingTimerRight is a count down number for the right trigger
	float shootingTimerRight;
	//shootingTimerLeft is a count down number for the left trigger
	float shootingTimerLeft;
	//timeBetweenShots is the delay between bullets spawning
	public float timeBetweenShots = 0.02f;

	//rotationHolder is used to hold the rotation of the player
	public Vector3 rotationHolder = Vector3.zero;

	//build selection menu
	//canvasReference is a reference to the player's canvas
	public GameObject canvasReference;

	//BuildButton is a class for UI buttons
	//Serializable allows me to create BuildButton objects in the inspector
	[System.Serializable]
	public class BuildButton {
		public string title;
		public Image image;
		public Color normalColour;
		public Color highlightedColour;
	}

	//BuildOptions is a list of BuildButton objects
	public List<BuildButton> buildOptions = new List<BuildButton> ();

	//current menu index
	int currentMenuOption;
	//last menu index
	int oldMenuOption;
	//bool check if left trigger is active
	bool leftTriggerActive;

	//a reference to buildingManager object of BuildingManager class
	BuildingManager buildingManager;

	//placing object
	//bool check if released trigger button
	bool triggerUp;
	//bool check if currently building
	public bool isBuilding = false;
	//bool check if we can open build menu
	public static bool canOpenBuildMenu = true;

	//Player states
	//enum of player states
	public enum state {NotBuilding, Selecting, Placing};
	//playerState is a state object and is set to NotBuilding
	public state playerState = state.NotBuilding;

	//player index
	[HideInInspector]
	public int playerID;

	//Events
	//onPlace is an event called when place object button is pressed
	public delegate void OnPlaced(int p_ID);
	public static event OnPlaced onPlace;
	//onExit is an event called when exiting the placing state
	public delegate void OnExited(int p_ID);
	public static event OnExited onExit;

	//--------------------------------------------------------------------------------------
	//	Start()
	// Runs during initialisation
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Start() {
		if (controller == XboxController.First) {
			playerID = 1;
		} else {
			if (controller == XboxController.Second) {
				playerID = 2;
			}
		}
		Debug.Log(playerID);
		rb = GetComponent<Rigidbody> ();
		rotationHolder = transform.GetChild (0).rotation.eulerAngles;
		buildingManager = GameObject.FindGameObjectWithTag ("BuildingManager").GetComponent<BuildingManager>();
		if (controller == XboxController.First) {
			playerID = 1;
		} else if (controller == XboxController.Second) {
			playerID = 2;
		}
	}

	//--------------------------------------------------------------------------------------
	//	PlayerMove()
	// Moves the player, player jumping, open build menu
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void PlayerMove() {
		//move player
		float axisX = XCI.GetAxis (XboxAxis.LeftStickX, controller);
		float axisZ = XCI.GetAxis (XboxAxis.LeftStickY, controller);

		Vector3 movement = new Vector3 (axisX, 0, axisZ);
		transform.Translate (movement * moveSpeed * Time.deltaTime);

		//jump
		CheckGrounded();
		if (XCI.GetButtonDown(XboxButton.A, controller) && CheckGrounded()) {
			rb.velocity = Vector3.up * jumpVelocity;
		}
		//if falling, fall faster, else if moving up and not pressing jump start falling
		if (rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (rb.velocity.y > 0 && !XCI.GetButton(XboxButton.A, controller)) {
			rb.velocity = Vector3.zero;
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		//Open building menu
		if ((XCI.GetAxis(XboxAxis.LeftTrigger, controller) > 0) && !isBuilding && canOpenBuildMenu) {
			playerState = state.Selecting;
		}
	}

	//--------------------------------------------------------------------------------------
	//	PlayerTurn()
	// Rotates player left and right, and looks up and down with in a range
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void PlayerTurn() {
		//rotate player
		float rotateAxisY = XCI.GetAxis (XboxAxis.RightStickX, controller);
		float rotateAxisX = XCI.GetAxis (XboxAxis.RightStickY, controller);

		Vector3 turnVector = new Vector3 (0, rotateAxisY, 0);
		transform.Rotate (turnVector * turnSpeed * Time.deltaTime);

		//look up and down
		Vector3 lookVector = new Vector3 (-rotateAxisX, 0, 0);
		rotationHolder += lookVector;
		rotationHolder.x = Mathf.Clamp (rotationHolder.x, -26, 26);
		transform.GetChild (0).transform.localRotation = Quaternion.Euler(rotationHolder.x, transform.rotation.y, 0);
	}

	//--------------------------------------------------------------------------------------
	//	CheckGrounded()
	// Check if player is standing, return true if raycast hit something below player
	//
	// Param:
	//		None
	// Return:
	//		Bool
	//--------------------------------------------------------------------------------------
	//return true if raycast hit ground
	bool CheckGrounded() {
		float rayLength = 1.1f;
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
		// if there is something directly below the player
		return Physics.Raycast(ray, out hit, rayLength);
	}

	//--------------------------------------------------------------------------------------
	//	FireGun()
	// Shoot when trigger down with a delay between bullets spawning
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void FireGun() {
		if (XCI.GetAxis (XboxAxis.RightTrigger, controller) > 0.1f) {
			if (Time.time - shootingTimerRight > timeBetweenShots) {
				GameObject GO = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
				GO.transform.SetParent (bulletParent);
				GO.GetComponent<Rigidbody> ().AddForce (bulletSpawn.transform.forward * bulletSpeed, ForceMode.Impulse);
				Destroy (GO, 3);
				shootingTimerRight = Time.time;
			}
		}
	}

	//--------------------------------------------------------------------------------------
	//	Selecting()
	// Opens Build select UI, change to placing state if an option was selected on trigger up
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Selecting() {
		leftTriggerActive = true;
		canvasReference.SetActive (true);
		GetCurrentMenuOption ();

		if ((XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0)) {
			canvasReference.SetActive (false);
			leftTriggerActive = false;
			if (currentMenuOption != 0) {
				playerState = state.Placing;
			}
			else {
				playerState = state.NotBuilding;
			}
		}
	}

	//--------------------------------------------------------------------------------------
	//	GetCurrentMenuOption()
	// Select a menu option with right analog stick, deselect last option selected when a
	// new option is selected
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void GetCurrentMenuOption() {
		if (XCI.GetAxis(XboxAxis.RightStickY, controller) > 0 && leftTriggerActive == true) {
			currentMenuOption = 1;
		}
		if (XCI.GetAxis(XboxAxis.RightStickY, controller) < 0 && leftTriggerActive == true) {
			currentMenuOption = 3;
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller) > 0 && leftTriggerActive == true) {
			currentMenuOption = 2;
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller) < 0 && leftTriggerActive == true) {
			currentMenuOption = 4;
		}
		if ((XCI.GetAxis(XboxAxis.RightStickX, controller) == 0) && (XCI.GetAxis(XboxAxis.RightStickY, controller) == 0)) {
			currentMenuOption = 0;
		}

		if ((currentMenuOption != oldMenuOption) && currentMenuOption != 0) {
			buildOptions [oldMenuOption].image.color = buildOptions [oldMenuOption].normalColour;
			oldMenuOption = currentMenuOption;
			buildOptions [currentMenuOption].image.color = buildOptions [currentMenuOption].highlightedColour;
		}
		if ((currentMenuOption != oldMenuOption) && currentMenuOption == 0) {
			buildOptions [oldMenuOption].image.color = buildOptions [oldMenuOption].normalColour;
			oldMenuOption = currentMenuOption;
		}
	}

	//--------------------------------------------------------------------------------------
	//	Placing()
	// Ignore menu options 4 and 1 and go back to NotBuilding state, if not currently building
	// call BuildObject(), on right trigger up call onPlace event
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Placing() {
		if (currentMenuOption == 1) {
			Debug.Log("No Trap or Turret Yet!");
			playerState = state.NotBuilding;
			return;
		} else if (!isBuilding) {
			isBuilding = true;
			buildingManager.BuildObject (currentMenuOption, playerID);
			currentMenuOption = 0;
		}

		if(XCI.GetButtonDown(XboxButton.B, controller)) {
			if (onExit != null) {
				onExit (playerID);
			}
		}
		if(XCI.GetAxisRaw(XboxAxis.RightTrigger, controller) > 0) {
			triggerUp = true;
		}
		if((XCI.GetAxisRaw(XboxAxis.RightTrigger, controller) == 0) && triggerUp) {
			triggerUp = false;
			if (onPlace != null) {
				onPlace (playerID);
			}
		}
	}


	//--------------------------------------------------------------------------------------
	//	Update()
	// Runs every frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update() {

		PlayerMove ();

		switch (playerState) {
		case state.NotBuilding:
			PlayerTurn ();
			FireGun ();
			break;
		case state.Selecting:
			Selecting ();
			break;
		case state.Placing:
			PlayerTurn ();
			Placing ();
			break;
		}
	}


	//--------------------------------------------------------------------------------------
	//	TakeDamage()
	// lose health
	//
	// Param:
	//		damage - how much health to lose
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void TakeDamage(int damage) {
		playerHealth -= damage;
		if(playerHealth <= 0) {
			Debug.Log (gameObject.name + " died, end game");
			SceneManager.LoadScene ("MainMenu");
		}
	}
}
