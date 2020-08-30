using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D col) {

		if (col.tag == "Projectile") {
			Destroy (col.gameObject);
			Destroy (this.gameObject);
		}

	}
}
