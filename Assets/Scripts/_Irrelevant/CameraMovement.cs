using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform target;
	public float distance = 10.0f;
	public float cameraSpeed = 5;

	public float xSpeed = 175.0f;
	public float ySpeed = 75.0f;

	public float yMinLimit = 20; //Lowest vertical angle in respect with the target.
	public float yMaxLimit = 80;

	public float minDistance = 5; //Min distance of the camera from the target
	public float maxDistance = 20;

	private float x = 0.0f;
	private float y = 0.0f;

	protected void Start()
	{
		var angles = Camera.main.transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	protected void LateUpdate()
	{
		//Zooming with mouse
		distance += -Input.GetAxis("Mouse ScrollWheel") * distance;
		distance = Mathf.Clamp(distance, minDistance, maxDistance);

		//Detect mouse drag;
		if (Input.GetMouseButton(1))
		{

			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}

		y = ClampAngle(y, yMinLimit, yMaxLimit);

		var rotation = Quaternion.Euler(y, x, 0);
		var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

		//When the target changed or moved, Move the camera smoothly with it.       
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position, cameraSpeed * Time.deltaTime);
		Camera.main.transform.rotation = rotation;
		Camera.main.transform.LookAt(target);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}
