using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

class Player : MonoBehaviour {
	public float moveSpeed = 1f;
	public float gridSize = 0.32f;
	public Grid grid;
	public Tilemap map;
	public Tilemap colliders;
	public Tile clearTile;

	private Vector3 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;
	private float maxY;

	public void Start () {
		maxY = grid.CellToLocal (map.cellBounds.max).y + 0.32f;
	}

	public void Update () {
		if (!isMoving) {
			input = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			if (Mathf.Abs (input.x) > Mathf.Abs (input.y)) {
				input.y = 0;
			}
			else {
				input.x = 0;
			}

			if (input != Vector3.zero) {
				if (!(input == Vector3.up && (transform.position.y + 0.32f) >= maxY)) {
					if (ValidTile(grid.WorldToCell(transform.position + input * 0.32f))) {
						StartCoroutine (move (transform));
					}
				}
			}
		}
	}

	public IEnumerator move (Transform transform) {
		isMoving = true;
		startPosition = transform.position;
		t = 0;
		endPosition = new Vector3 (startPosition.x + System.Math.Sign (input.x) * gridSize,
			startPosition.y + System.Math.Sign (input.y) * gridSize, startPosition.z);

		factor = 1f;

		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed / gridSize) * factor;
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
			Vector3Int coordinate = grid.WorldToCell (transform.position);
			TileBase tile = map.GetTile (coordinate);
			if (tile != null && tile != clearTile) {
				map.SetTile (coordinate, clearTile);
			}

			yield return null;
		}

		isMoving = false;
		yield return 0;
	}

	private bool ValidTile (Vector3Int tilePosInCells) {
		return colliders.GetTile (tilePosInCells) == null;
	}
}
