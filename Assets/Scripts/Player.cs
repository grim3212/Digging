using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {
	public float moveSpeed = 1f;

	private Vector3 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;

	public GameObject projectile;

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
				if (ValidTile (World.Instance.Grid.WorldToCell (transform.position + input))) {
					StartCoroutine (move (transform));
				}
			}
		}

		//Detect when mouse is clicked
		if (Input.GetMouseButtonDown (0)) {
			Instantiate (this.projectile, transform.position, transform.rotation);
		}
	}

	public IEnumerator move (Transform transform) {

		isMoving = true;
		startPosition = transform.position;
		t = 0;
		endPosition = new Vector3 (startPosition.x + System.Math.Sign (input.x) * World.Instance.GridSize,
			startPosition.y + System.Math.Sign (input.y) * World.Instance.GridSize, startPosition.z);

		factor = 1f;

		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed / World.Instance.GridSize) * factor;
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
			Vector3Int coordinate = World.Instance.Grid.WorldToCell (transform.position);
			TileBase tile = World.Instance.Map.GetTile (coordinate);
			if (tile != null && tile != World.Instance.ClearTile) {
				World.Instance.Map.SetTile (coordinate, World.Instance.ClearTile);
			}

			yield return null;
		}

		isMoving = false;
		yield return 0;
	}

	private bool ValidTile (Vector3Int tilePosInCells) {
		return World.Instance.Colliders.GetTile (tilePosInCells) == null;
	}
	
}
