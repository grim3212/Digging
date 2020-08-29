using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed = 1.0f;
	private Vector3 direction;

	void Start () {
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		direction = (mouseWorldPos - transform.position);
		direction.z = 0;
		direction.Normalize ();

		float angle = Util.AngleBetweenPoints (mouseWorldPos, transform.position);
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
	}

	void Update () {
		transform.position = transform.position + (direction * speed * Time.deltaTime);
	}

}
