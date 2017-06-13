using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//reference to UI image for buildmode timer
	public Image buildModeTimerBar;
	//how long build mode lasts
	public float buildModeLength = 10;
	//current buildmode timer
	public float buildModeTimer { get; set; }

	//reference to wave spawner component
	WaveSpawner waveSpawner;
	//check if build mode is active
	public static bool isBuildMode = true;

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
		waveSpawner = GetComponent<WaveSpawner> ();
		buildModeTimer = buildModeLength;
		buildModeTimerBar.color = new Color32(29, 148, 248, 255);
		buildModeTimerBar.transform.GetChild(0).GetComponent<Text>().text = "Build Mode";
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
		if (buildModeTimer > 0) {
			buildModeTimer -= Time.deltaTime;
			buildModeTimerBar.fillAmount = buildModeTimer / buildModeLength;
		}
		if (buildModeTimer <= 0) {
			buildModeTimerBar.fillAmount = 1;
			buildModeTimerBar.color = new Color32(163, 17, 0, 255); 
			PlayerController player1 = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController> ();
			PlayerController player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController> ();
			player1.canOpenBuildMenu = false;
			player2.canOpenBuildMenu = false;
			if(isBuildMode && !player1.isBuilding && !player2.isBuilding) {
				isBuildMode = false;
				//grid.SetGridActive ();
				waveSpawner.TurnOnWaveSpawner ();
			}
		}
	}
}
