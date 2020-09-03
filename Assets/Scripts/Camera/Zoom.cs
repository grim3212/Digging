
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {
	public Camera ppwzCamera;
	private PixelPerfect ppwz;

	void Start () {
		ppwz = ppwzCamera.GetComponent<PixelPerfect> ();
	}

	void Update () {
		if (Input.mouseScrollDelta.y != 0) {
			if (Input.mouseScrollDelta.y > 0) {
				ppwz.ZoomIn ();
			}
			else {
				ppwz.ZoomOut ();
			}
		}
	}
}