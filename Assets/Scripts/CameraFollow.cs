using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform player;
    public float smoothSpeed = 0.5f;

	private Vector2 worldMin;
    private Vector2 worldMax;
	private float camY, camX;
	private float camOrthsize;
	private float cameraRatio;
	private Camera mainCam;
    private Vector3 smoothPos;

	void Start () {
        Bounds localBounds = World.Instance.Colliders.localBounds;
        worldMin = World.Instance.Colliders.transform.TransformPoint(localBounds.min);
        worldMax = World.Instance.Colliders.transform.TransformPoint(localBounds.max);
		
		mainCam = GetComponent<Camera> ();
		camOrthsize = mainCam.orthographicSize;
		cameraRatio = (worldMax.x + camOrthsize) / 2.0f;
	}

	void LateUpdate () {
		//camY = Mathf.Clamp (this.player.position.y, worldMin.y + camOrthsize, worldMax.y - camOrthsize);
		camX = Mathf.Clamp (this.player.position.x, worldMin.x + cameraRatio, worldMax.x - cameraRatio);
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, this.player.position.y, this.transform.position.z), smoothSpeed);
		this.transform.position = smoothPos;
	}
}
