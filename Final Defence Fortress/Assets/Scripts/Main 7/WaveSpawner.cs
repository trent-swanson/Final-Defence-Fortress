using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour {

	//refence to transform of enemy parent object
	public Transform enemyParent;
	//reference to UI text object
	public Text waveText;

	//enum of spawn states
	public enum SpawnState {Spawning, Waiting, NotActive, Start};
	SpawnState state = SpawnState.NotActive;

	//wave class
	[System.Serializable]
	public class Wave {
		public string name;
		public GameObject enemyPrefab;
		public int count;
		public float rate;
	}

	//array of wave objects
	public Wave[] waves;
	//next wave index
	int nextWave = 0;
	//time between weach wave
	public float timeBetweenWaves = 5;
	//current count down timer
	float waveCountDown;

	//array of spawn point transforms
	public Transform[] spawnPoints;

	//timer amount for searching for enemies alive
	float searchCountDown = 1;
	//current wave number
	int waveCount = 0;

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
		if(spawnPoints.Length == 0) {
			Debug.Log ("No Spawn Points referenced");
		}
		waveCountDown = timeBetweenWaves;
	}

	//--------------------------------------------------------------------------------------
	//	TurnOnWaveSpawner()
	// Change spawnstate to start
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	public void TurnOnWaveSpawner() {
		state = SpawnState.Start;
	}

	//--------------------------------------------------------------------------------------
	//	Update()
	// runs every frame
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void Update() {
		if (waveCount == 4) {
			Debug.Log ("YOU WON!!");
			Cursor.visible = true;
			SceneManager.LoadScene ("MainMenu");
		}
		switch(state) {
		case SpawnState.Waiting:
			CountDown ();
			break;
		case SpawnState.Start:
			StartCoroutine (SpawnWave (waves [nextWave]));
			break;
		case SpawnState.NotActive:
			break;
		}
	}

	//--------------------------------------------------------------------------------------
	//	CountDown()
	// Countdown time between waves
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void CountDown() {
		if(waveCountDown <= 0) {
			if(state != SpawnState.Spawning) {
				if(!EnemyIsAlive()) {
					waveCountDown = timeBetweenWaves;
					if(nextWave + 1 > waves.Length - 1) {
						nextWave = 0;
						Debug.Log ("All Waves Complete! Looping...");
					} else {
						nextWave++;
					}
					// start spawning wave
					StartCoroutine(SpawnWave(waves[nextWave]));
				} else {
					return;
				}
			}
		} else {
			waveCountDown -= Time.deltaTime;
		}
	}

	//--------------------------------------------------------------------------------------
	//	EnemyIsAlive()
	// Check if any enemies are still alive
	//
	// Param:
	//		None
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	bool EnemyIsAlive() {
		searchCountDown -= Time.deltaTime;
		if (searchCountDown <= 0) {
			searchCountDown = 1;
			if (GameObject.FindGameObjectWithTag("Enemy") == null) {
				return false;
			}
		}
		return true;
	}

	//--------------------------------------------------------------------------------------
	//	SpawnWave()
	// Coroutine: spawn enemies for this wave with a delay between each
	//
	// Param:
	//		p_wave: wave object reference
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	IEnumerator SpawnWave(Wave p_wave) {
		waveCount++;
		Debug.Log ("Spawning Wave: " + waveCount + ":  " + p_wave.name);
		waveText.GetComponent<Text>().text = "Wave " + waveCount + "  " + p_wave.name;
		state = SpawnState.Spawning;
		for (int i = 0; i < p_wave.count; i++) {
			SpawnEnemy (p_wave.enemyPrefab);
			yield return new WaitForSeconds (1 / p_wave.rate);
		}
		state = SpawnState.Waiting;
		yield break;
	}

	//--------------------------------------------------------------------------------------
	//	SpawnEnemy()
	// Instantiate enemy object
	//
	// Param:
	//		p_enemy: enemy object reference
	// Return:
	//		Void
	//--------------------------------------------------------------------------------------
	void SpawnEnemy(GameObject p_enemy) {
		// spawn enemy
		Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		GameObject GO = Instantiate(p_enemy, sp.position, sp.rotation);
		GO.transform.SetParent (enemyParent);
	}
}
