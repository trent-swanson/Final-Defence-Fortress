using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState {Spawning, Waiting, Counting, NotActive};
	SpawnState state = SpawnState.NotActive;

	[System.Serializable]
	public class Wave {
		public string name;
		public GameObject enemyPrefab;
		public int count;
		public float rate;
	}

	public Wave[] waves;
	int nextWave = 0;
	public float timeBetweenWaves = 5;
	float waveCountDown;

	public Transform[] spawnPoints;

	float searchCountDown = 1;

	void Start() {
		if(spawnPoints.Length == 0) {
			Debug.Log ("No Spawn Points referenced");
		}
		waveCountDown = timeBetweenWaves;
	}

	public void TurnOnWaveSpawner() {
		state = SpawnState.Waiting;
	}

	void Update() {
		if(state != SpawnState.NotActive) {
			if (state == SpawnState.Waiting) {
				if(!EnemyIsAlive()) {
					// new wave
					state = SpawnState.Counting;
					waveCountDown = timeBetweenWaves;
					if(nextWave + 1 > waves.Length - 1) {
						nextWave = 0;
						Debug.Log ("All Waves Complete! Looping...");
					} else {
						nextWave++;
					}
				} else {
					return;
				}
			}


			if(waveCountDown <= 0) {
				if(state != SpawnState.Spawning) {
					// start spawning wave
					StartCoroutine(SpawnWave(waves[nextWave]));
				}
			} else {
				waveCountDown -= Time.deltaTime;
			}
		}
	}

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

	IEnumerator SpawnWave(Wave p_wave) {
		Debug.Log ("Spawning Wave" + p_wave.name);
		state = SpawnState.Spawning;
		for (int i = 0; i < p_wave.count; i++) {
			SpawnEnemy (p_wave.enemyPrefab);
			yield return new WaitForSeconds (1 / p_wave.rate);
		}
		state = SpawnState.Waiting;
		yield break;
	}

	void SpawnEnemy(GameObject p_enemy) {
		// spawn enemy
		Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(p_enemy, sp.position, sp.rotation);
	}
}
