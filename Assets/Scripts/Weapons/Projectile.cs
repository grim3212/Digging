using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed = 1.0f;
	private Vector3 direction;
	private bool IsMoving = true;

	void Start () {
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		direction = (mouseWorldPos - transform.position);
		direction.z = 0;
		direction.Normalize ();

		float angle = Util.AngleBetweenPoints (mouseWorldPos, transform.position);
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
	}

	void Update () {
		Vector3 nextPos = transform.position + (direction * speed * Time.deltaTime);

		if (IsMoving) {
			if (ValidTile (World.Instance.Map.WorldToCell (nextPos))) {
				transform.position = nextPos;
			}
			else {
				IsMoving = false;
			}
		}


		if (!IsMoving) {
			// Destroy immediately if the stopping tile is destroyed
			if (ValidTile (World.Instance.Map.WorldToCell (nextPos))) {
				Destroy (this.gameObject);
			}
			// Destroy in 1 second
			Destroy (this.gameObject, 1.0f);
		}

	}


	private bool hasCollided = false;
	void OnTriggerEnter2D (Collider2D col) {
		if (col.name == "Player") { return; }
		if (this.hasCollided == true) { return; }

		this.hasCollided = true;

		if (col.tag == "Enemy") {
			Destroy (col.gameObject);
			Destroy (this.gameObject);
			Instantiate (World.Instance.BloodParticles, col.gameObject.transform.position, col.gameObject.transform.rotation);
			World.Instance.gameManager.score += 100;
		}
	}
	void LateUpdate () {
		this.hasCollided = false;
	}

	private bool ValidTile (Vector3Int tilePosInCells) {
		return World.Instance.Map.GetTile (tilePosInCells) == World.Instance.ClearTile && World.Instance.Colliders.GetTile (tilePosInCells) == null;
	}

}
