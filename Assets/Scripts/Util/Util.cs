using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

	/*
    * The returned angle will depend on the order you give for a and b
    */
	public static float AngleBetweenPoints (Vector2 a, Vector2 b) {
		return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}


}
