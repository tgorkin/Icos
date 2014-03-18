using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Camera controller, pans and zooms the camera based on user input.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
	private const string INPUT_AXIS_VERTICAL = "Vertical";
	private const string INPUT_AXIS_MOUSE_X = "Mouse X";
	private const string INPUT_AXIS_MOUSE_Y = "Mouse Y";
	private const string INPUT_AXIS_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
	private const string INPUT_BUTTON_FIRE_1 = "Fire1";
	
	public float PanSpeed = 1f;
	public float ZoomSpeed = 1f;
	public float YMinLimit = -20f;
	public float YMaxLimit = 80f;
	public float DistanceMin = .5f;
	public float DistanceMax = 15f;

	public Transform target;

	float x;
	float y;
	float distance;

	void Start () {
		transform.LookAt ( target.position );
		distance = Vector3.Magnitude (transform.position - target.position);
	}

	void LateUpdate () {
		if ( target ) {
		    if( Input.GetButton(INPUT_BUTTON_FIRE_1) ) {
				x -= Input.GetAxis (INPUT_AXIS_MOUSE_X) * PanSpeed;
				y += Input.GetAxis (INPUT_AXIS_MOUSE_Y) * PanSpeed;
				y = ClampAngle (y, YMinLimit, YMaxLimit);
			}

			Quaternion rotation = Quaternion.Euler (y, x, 0);

			distance = Mathf.Clamp (distance - Input.GetAxis (INPUT_AXIS_MOUSE_SCROLLWHEEL) * ZoomSpeed, DistanceMin, DistanceMax);

			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) {
					distance -= hit.distance;
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max){
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
