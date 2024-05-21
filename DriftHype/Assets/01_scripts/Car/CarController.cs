using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	[Range(0f, 180f)]
	[SerializeField] private float accelTresholdAngle = 5f;

	[Header("Turn")]
    [SerializeField] private float turnRate;

    [Header("References")]
    private Rigidbody rigid;

	[Header("Flags")]
	private float desireAngle;
	private float currentAngle;
	private float currentSpeed;
	private Vector3 velocity;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		accelTresholdAngle *= Mathf.Deg2Rad;
	}

	private void FixedUpdate()
	{
		Turn();
		Move();
	}

	private void Move()
	{
		velocity = Vector3.zero;

		float radAngle = Mathf.Deg2Rad * (-desireAngle + 90f);
		Vector3 desireDir = new Vector3(Mathf.Cos(radAngle), 0 , Mathf.Sin(radAngle));
		Vector3 velocityDir = rigid.velocity.normalized;
		float angleDot = Vector3.Dot(desireDir, velocityDir);

		if (rigid.velocity == Vector3.zero || (1 - Mathf.Abs(angleDot)) < accelTresholdAngle)
		{
			print("가속");
			velocity = transform.forward * CalculatedSpeed();
			rigid.velocity = velocity;
		}
		else
		{
			print("감속");
			Vector3 dragDir = Vector3.zero;
			Vector3 n = Vector3.Cross(transform.forward, velocityDir);
			dragDir = Vector3.Cross(transform.forward, n);
			velocity = rigid.velocity;
			velocity += dragDir;
			rigid.velocity = velocity;
		}

		Debug.DrawRay(transform.position, desireDir * 50f, Color.red); // 핸들 방향
		Debug.DrawRay(transform.position, velocityDir * rigid.velocity.magnitude, Color.green); // 힘의 방향
		Debug.DrawRay(transform.position, transform.forward * currentSpeed, Color.blue); // 객체 방향
	}

	private float CalculatedSpeed()
	{
		float angleBetween = Vector3.Angle(transform.forward, rigid.velocity.normalized);
		float cosBetween = Mathf.Cos(angleBetween * Mathf.Deg2Rad);
		float speedForward = cosBetween * rigid.velocity.magnitude;
		currentSpeed = speedForward;

		if (currentSpeed <= maxSpeed)
		{
			currentSpeed += acceleration * Time.fixedDeltaTime;
			currentSpeed = Mathf.Clamp(currentSpeed, float.MinValue, maxSpeed);
		}

		return currentSpeed;
	}

	public void SetDesireAngle(float angle)
	{
		desireAngle = angle;
	}
	
	private void Turn()
	{
		float rotY = transform.eulerAngles.y;
		rotY = rotY > 180 ? rotY - 360f : rotY;
		float sign = Mathf.Sign(desireAngle - rotY);
		float lerpAngle = rotY + sign * turnRate * Time.fixedDeltaTime;
		if (sign != Mathf.Sign(desireAngle - lerpAngle))
		{
			lerpAngle = desireAngle;
		}

		transform.rotation = Quaternion.Euler(0, lerpAngle, 0);
	}
}
