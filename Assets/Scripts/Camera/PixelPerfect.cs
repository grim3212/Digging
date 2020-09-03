using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfect : MonoBehaviour {
	[SerializeField]
	private Transform player;
	[SerializeField]
	private float smoothFollowSpeed = 1f;

	private Vector2 worldMin;
	private Vector2 worldMax;
	private float camY, camX;
	private float camOrthsize;
	private float cameraRatio;
	private Vector3 smoothPos;

	[SerializeField]
	private float pixelsPerUnit = 32;
	[SerializeField] // Uncomment if you want to watch scaling in the editor
	private float pixelsPerUnitScale = 1;
	[SerializeField]
	private float zoomScaleMax = 10f;
	[SerializeField]
	private float zoomScaleStart = 1f;
	[SerializeField]
	private float zoomScaleMin = 1f;
	[SerializeField]
	private bool smoovZoom = true;
	[SerializeField]
	private float smoovZoomDuration = 0.5f; // In seconds

	private int screenHeight;

	private float cameraSize;
	private Camera cameraComponent;

	private float zoomStartTime = 0f;
	private float zoomCurrentValue = 1f;
	private float zoomNextValue = 1f;
	private float zoomInterpolation = 1f;

	public float currentZoomScale { get { return pixelsPerUnitScale; } }

	void Start () {
		Bounds localBounds = World.Instance.Colliders.localBounds;
		worldMin = World.Instance.Colliders.transform.TransformPoint (localBounds.min);
		worldMax = World.Instance.Colliders.transform.TransformPoint (localBounds.max);

		screenHeight = Screen.height;
		cameraComponent = gameObject.GetComponent<Camera> ();
		cameraComponent.orthographic = true;
		SetZoomImmediate (zoomScaleStart);
	}

	void Update () {
		if (screenHeight != Screen.height) {
			screenHeight = Screen.height;
			UpdateCameraScale ();
		}

		if (midZoom) {
			if (smoovZoom) {
				zoomInterpolation = (Time.time - zoomStartTime) / smoovZoomDuration;
			}
			else {
				zoomInterpolation = 1; // express to the end
			}
			pixelsPerUnitScale = Mathf.Lerp (zoomCurrentValue, zoomNextValue, zoomInterpolation);
			UpdateCameraScale ();
		}
	}

	void LateUpdate () {
		// These values could change with the zoom
		camOrthsize = cameraComponent.orthographicSize;
		cameraRatio = cameraComponent.aspect * camOrthsize;

		camY = Mathf.Clamp (this.player.position.y, worldMin.y + camOrthsize, worldMax.y - camOrthsize);
		camX = Mathf.Clamp (this.player.position.x, worldMin.x + cameraRatio, worldMax.x - cameraRatio);
		smoothPos = Vector3.Lerp (this.transform.position, new Vector3 (camX, camY, this.transform.position.z), smoothFollowSpeed);
		this.transform.position = new Vector3 (camX, camY, this.transform.position.z);
	}

	private void UpdateCameraScale () {
		// The magic formular from teh Unity Docs
		cameraSize = (screenHeight / (pixelsPerUnitScale * pixelsPerUnit)) * 0.5f;
		cameraComponent.orthographicSize = cameraSize;
	}

	private bool midZoom { get { return zoomInterpolation < 1; } }

	private void SetUpSmoovZoom () {
		zoomStartTime = Time.time;
		zoomCurrentValue = pixelsPerUnitScale;
		zoomInterpolation = 0f;
	}

	public void SetPixelsPerUnit (float pixelsPerUnitValue) {
		pixelsPerUnit = pixelsPerUnitValue;
		UpdateCameraScale ();
	}

	// Has to be >= zoomScaleMin
	public void SetZoomScaleMax (int zoomScaleMaxValue) {
		zoomScaleMax = Mathf.Max (zoomScaleMaxValue, zoomScaleMin);
	}

	public void SetSmoovZoomDuration (float smoovZoomDurationValue) {
		smoovZoomDuration = Mathf.Max (smoovZoomDurationValue, 0.0333f); // 1/30th of a second sounds small enough
	}

	// Clamped to the range [1, zoomScaleMax], Integer values will be pixel-perfect
	public void SetZoom (float scale) {
		SetUpSmoovZoom ();
		zoomNextValue = Mathf.Max (Mathf.Min (scale, zoomScaleMax), zoomScaleMin);
	}

	// Clamped to the range [1, zoomScaleMax], Integer values will be pixel-perfect
	public void SetZoomImmediate (float scale) {
		pixelsPerUnitScale = Mathf.Max (Mathf.Min (scale, zoomScaleMax), zoomScaleMin);
		UpdateCameraScale ();
	}

	public void ZoomIn () {
		if (!midZoom) {
			SetUpSmoovZoom ();
			zoomNextValue = Mathf.Min (pixelsPerUnitScale + 1, zoomScaleMax);
		}
	}

	public void ZoomOut () {
		if (!midZoom) {
			SetUpSmoovZoom ();
			zoomNextValue = Mathf.Max (pixelsPerUnitScale - 1, zoomScaleMin);
		}
	}
}