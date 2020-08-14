using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour {
	public float moveSpeed = 1f;
	public float gridSize = 0.32f;
	public Grid grid;
	public Tilemap map;
	public Tile clearTile;

	private Vector2 input;
	private Vector3Int previousDirection;
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
			Vector3Int coordinate = grid.WorldToCell (transform.position);
			if (previousDirection != Vector3Int.zero && ValidTile (map.GetTile (coordinate + previousDirection))) {
				input = new Vector2 (previousDirection.x, previousDirection.y);
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.up))) {
				input = Vector2.up;
				previousDirection = Vector3Int.up;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.left))) {
				input = Vector2.left;
				previousDirection = Vector3Int.left;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.down))) {
				input = Vector2.down;
				previousDirection = Vector3Int.down;
			}
			else if (ValidTile (map.GetTile (coordinate + Vector3Int.right))) {
				input = Vector2.right;
				previousDirection = Vector3Int.right;
			}
			else {
				input = Vector2.zero;
				previousDirection = Vector3Int.zero;
			}

			if (input != Vector2.zero) {
				if (!(input == Vector2.up && (transform.position.y + 0.32f) >= maxY)) {
                    // TODO: Why doesnt this raycast work?
                    RaycastHit2D hit = Physics2D.Raycast (transform.position, input, 0.32f);
					if (hit.collider == null) {
						StartCoroutine (move (transform));
					}else {
                        previousDirection = Vector3Int.zero;
                    }
				}
			}
		}
	}

	private bool ValidTile (TileBase tile) {
		return tile == clearTile;
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
			yield return null;
		}

		isMoving = false;
		yield return 0;
	}
}
