using UnityEngine;

public class HandleUI : MonoBehaviour
{
	[SerializeField] private CarController carController;
	[Tooltip("Degree Per Second")]
	[SerializeField] private float turnRate;
	[Range(-180f, 0f)] 
	[SerializeField] private float minAngle;
	[Range(0f, 180f)]
	[SerializeField] private float maxAngle;

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Vector2 touchPos = Input.GetTouch(0).position;
			Vector2 dir = touchPos - (Vector2)transform.position;

			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			angle = angle > 180f ? -360 + angle : angle;
			angle = Mathf.Clamp(angle, minAngle, maxAngle);
			//float rotZ = transform.eulerAngles.z;
			//rotZ = rotZ > 180 ? rotZ - 360f : rotZ;
			//float sign = Mathf.Sign(angle - rotZ);

			//float lerpAngle = rotZ + sign * turnRate * Time.deltaTime;
			//if (sign != Mathf.Sign(angle - lerpAngle))
			//{
			//	lerpAngle = angle;
			//}

			transform.rotation = Quaternion.Euler(0, 0, angle);
			carController.SetDesireAngle(-angle);
		}
		else if (Input.GetMouseButton(0))
		{
			Vector2 touchPos = Input.mousePosition;
			Vector2 dir = touchPos - (Vector2)transform.position;

			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
			angle = angle > 180f ? -360 + angle : angle;
			angle = Mathf.Clamp(angle, minAngle, maxAngle);

			transform.rotation = Quaternion.Euler(0, 0, angle);
			carController.SetDesireAngle(-angle);
		}
	}
}
