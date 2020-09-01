using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {
	private static World _instance;
	public static World Instance { get { return _instance; } }
	[HideInInspector]
	public string appPath;

	public float GridSize = 1f;
	public Grid Grid;
	public Tilemap Map;
	public Tilemap Colliders;
	public Tile ClearTile;
	public GameObject BloodParticles;

	[HideInInspector]
	public GameManager gameManager;

	public bool CanUpdate = true;

	public GameObject player;
	private TileBase[] allTiles;

	private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		}
		else {
			_instance = this;
		}

		appPath = Application.persistentDataPath;
		gameManager = new GameManager ();

		this.allTiles = Map.GetTilesBlock (Map.cellBounds);
	}

	private float timer = 0.0f;

	private void Update () {
		timer += Time.deltaTime;

		this.gameManager.currentTime = (int)timer % 60;

		if (this.gameManager.currentTime >= this.gameManager.totalTime) {
			// Time is up for this world
		}
	}

	public void ResetLevel () {
		this.player.GetComponent<Player> ().Reset ();
		EnemyManager.Instance.Reset ();
		Map.SetTilesBlock (Map.cellBounds, this.allTiles);
	}
}
