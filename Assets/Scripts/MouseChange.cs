using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseChange : MonoBehaviour {

	public Grid grid;
	public Tilemap tilemap;
	public Tile clearTile;

	// Update is called once per frame
	void Update () {

		//Detect when mouse is clicked
		if (Input.GetMouseButtonDown (0)) {

			//Get position of the mouseclick
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//Convert position of the mouseclick to the position of the tile located at the mouseclick
			Vector3Int coordinate = grid.WorldToCell (mouseWorldPos);
			//Display tile position in log
			Debug.Log (coordinate);
			//Display the sprite value of the tile in log *SUCCESS*
			Debug.Log (tilemap.GetSprite (coordinate));
			//Tile tile = tilemap.GetTile(coordinate);
			tilemap.SetTile (coordinate, clearTile);
			//tilemap.RefreshTile(coordinate);
		}
	}
}
