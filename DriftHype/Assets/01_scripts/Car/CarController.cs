using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	[Range(0f, 180f)]
	[SerializeField] private float accelAngle = 5f;

	[Header("Turn")]
    [SerializeField] private float turnRate;

    [Header("References")]
    private Rigidbody rigid;

	[Header("Flags")]
	private float angle;
	private Vector3 velocity;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		accelAngle *= Mathf.Deg2Rad;
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		velocity = Vector3.zero;

		/*
		 * ��ǥ ���� ���ϱ�
		 * ���� ���� = rigid.Velocity
		 * ��ǥ ����� ���� ������ ���� �� ���ϱ�
		 * ���� *�� �����ΰ�?
		 * true) ����
		 * false) ���� �ٲٱ�
		 */

		Vector3 desireDir = transform.forward;
		Vector3 velocityDir = rigid.velocity.normalized;
		float angleDot = Vector3.Dot(desireDir, velocityDir);
		if (rigid.velocity == Vector3.zero || (1 - Mathf.Abs(angleDot)) < accelAngle)
		{
			velocity = desireDir * CalculatedSpeed();
			print("����");
		}
		else
		{
			print("����");
		}

		rigid.velocity = velocity;
	}

	private float CalculatedSpeed()
	{
		return maxSpeed;
	}

	public void Turn(float angle)
	{
		float rotY = transform.eulerAngles.y;
		rotY = rotY > 180 ? rotY - 360f : rotY;
		float sign = Mathf.Sign(angle - rotY);
		float lerpAngle = rotY + sign * turnRate * Time.deltaTime;
		if (sign != Mathf.Sign(angle - lerpAngle))
		{
			lerpAngle = angle;
		}

		transform.rotation = Quaternion.Euler(0, lerpAngle, 0);
		this.angle = lerpAngle;
	}
}
