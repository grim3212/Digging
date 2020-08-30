using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
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

    private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		}
		else {
			_instance = this;
		}

		appPath = Application.persistentDataPath;
	}
}
