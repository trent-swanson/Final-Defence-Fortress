using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using XboxCtrlrInput;

public class RadialMenu : MonoBehaviour {

	public XboxController controller;

	[System.Serializable]
	public class BuildButton {
		public string title;
		public Image image;
		public Color normalColour = Color.white;
		public Color highlightedColour = Color.gray;
	}

	public GameObject radialCanvas;

	public List<BuildButton> buildOptions = new List<BuildButton> ();

	int currentMenuOption;
	int oldMenuOption;
	public static bool radialMenuActive;
	bool leftTriggerActive;

	BuildingManager buildingManager;

	bool triggerUp;

	void Start() {
		buildingManager = gameObject.GetComponent<BuildingManager> ();
		foreach(BuildButton button in buildOptions) {
			button.image.color = button.normalColour;
		}
		currentMenuOption = 0;
		oldMenuOption = 0;
	}

	void Update() {
		if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) > 0 && BuildingManager.isBuilding == false) {
			triggerUp = true;
			radialCanvas.SetActive (true);
			radialMenuActive = true;
		}
		GetCurrentMenuOption ();
		if ((XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0) && triggerUp) {
			triggerUp = false;
			radialCanvas.SetActive (false);
			radialMenuActive = false;
			if (currentMenuOption != 0) {
				ButtonAction ();
			}
		}
	}

	public void GetCurrentMenuOption() {

		if (XCI.GetAxis(XboxAxis.RightStickY, controller) > 0 && triggerUp == true) {
			currentMenuOption = 1;
		}
		if (XCI.GetAxis(XboxAxis.RightStickY, controller) < 0 && triggerUp == true) {
			currentMenuOption = 3;
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller) > 0 && triggerUp == true) {
			currentMenuOption = 2;
		}
		if (XCI.GetAxis(XboxAxis.RightStickX, controller) < 0 && triggerUp == true) {
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

	public void ButtonAction() {
		buildingManager.BuildObject (currentMenuOption);
		currentMenuOption = 0;
	}

}
