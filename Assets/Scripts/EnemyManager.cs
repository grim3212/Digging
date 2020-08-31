using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	private static EnemyManager _instance;
	public static EnemyManager Instance { get { return _instance; } }

	private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		}
		else {
			_instance = this;
		}
	}

	public void Reset () {
		Pathfinding[] enemies = GetComponentsInChildren<Pathfinding> ();

		foreach (Pathfinding enemy in enemies) {
			enemy.Reset ();
		}
	}
}
