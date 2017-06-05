using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Image buildModeTimerBar;
	public float buildModeLength = 10;
	public float buildModeTimer { get; set; }

	public Grid grid;

	WaveSpawner waveSpawner;
	public static bool isBuildMode = true;

	void Start() {
		waveSpawner = GetComponent<WaveSpawner> ();
		buildModeTimer = buildModeLength;
		buildModeTimerBar.color = new Color32(29, 148, 248, 255);
		buildModeTimerBar.transform.GetChild(0).GetComponent<Text>().text = "Build Mode";
	}

	void Update() {
		if (buildModeTimer > 0) {
			buildModeTimer -= Time.deltaTime;
			buildModeTimerBar.fillAmount = buildModeTimer / buildModeLength;
		}
		if (buildModeTimer <= 0) {
			buildModeTimerBar.fillAmount = 1;
			buildModeTimerBar.color = new Color32(163, 17, 0, 255); 
			PlayerController.canOpenBuildMenu = false;
			if(isBuildMode && !PlayerController.isBuilding) {
				isBuildMode = false;
				//grid.SetGridActive ();
				waveSpawner.TurnOnWaveSpawner ();
			}
		}
	}
}
