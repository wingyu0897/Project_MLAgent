using System;
using UnityEngine;

public class CarController : MonoBehaviour, ICar
{
	[Header("Movement")]
	[SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
	[Range(0f, 180f)]
	[SerializeField] private float accelThresholdAngle = 5f;
	[Range(0.01f, 1f)]
	[SerializeField] private float forwardLerpValue;
	[Range(0f, 1f)]
	[SerializeField] private float drag = 0.2f;
	[Range(0f, 1f)][SerializeField] private float dragAcceleration = 0.5f;

	[Header("Gravity")]
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float groundRayDistance = 1f;

	[Header("Turn")]
	[SerializeField] private float turnRate;

	[Header("Collision")]
	[SerializeField] private float bounceSpeed;
	[SerializeField] private float bouncePower = 5f;

	[Header("References")]
	private Rigidbody rigid;
	[HideInInspector] public ICarInput input;

	[Header("Flags")]
	private bool move = false;
	private float inputAngle;
	private float beforeSpeed;
	private float gravityVelocity;
	private Vector3 velocity;

	public GameObject NextTarget { get; set; }
	public bool IsPlayer { get; set; }

	public event Action<ICar, GameObject> OnTriggerCheckPoint;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		accelThresholdAngle *= Mathf.Deg2Rad;
	}

	private void FixedUpdate()
	{
		if (!move) return;
		Turn();
		//CheckGround();
		Move();
	}

	#region Movement
	private void Move()
	{
		velocity = Vector3.zero;

		float inputAngleRad = Mathf.Deg2Rad * (-inputAngle + 90f);
		Vector3 desireDir = new Vector3(Mathf.Cos(inputAngleRad), 0, Mathf.Sin(inputAngleRad));
		Vector3 velocityDir = rigid.velocity.normalized;
		float angleDot = Vector3.Dot(desireDir, velocityDir); // 내가 가고자 하는 방향과 현재 진행 방향의 격차(Cosθ)

		// 현재 멈추어 있거나, angleDot이 일정 각도 이하일 때
		if (rigid.velocity == Vector3.zero || (1 - Mathf.Abs(angleDot)) < accelThresholdAngle) // 가속
		{
			if (rigid.velocity == Vector3.zero)
			{
				velocity = transform.forward * CalculatedSpeed();
			}
			else
			{
				float cos = Vector3.Dot(transform.forward, velocityDir);

				float velocityAngle = Mathf.Atan2(velocityDir.z, velocityDir.x);
				velocityAngle = velocityAngle < -(Mathf.PI * 0.5f) ? 2 * Mathf.PI + velocityAngle : velocityAngle;
				float carDirAngle = Mathf.Atan2(transform.forward.z, transform.forward.x) - (cos <= 0f ? Mathf.PI : 0);
				carDirAngle = carDirAngle < -(Mathf.PI * 0.5f) ? 2 * Mathf.PI + carDirAngle : carDirAngle;
				
				float lerpAngle;
				if (velocityAngle > Mathf.PI && carDirAngle < 0)
					lerpAngle = Mathf.Lerp(velocityAngle, carDirAngle + Mathf.PI * 2f, forwardLerpValue);
				else if (carDirAngle > Mathf.PI && velocityAngle < 0)
					lerpAngle = Mathf.Lerp(velocityAngle, carDirAngle - Mathf.PI * 2f, forwardLerpValue);
				else
					lerpAngle = Mathf.Lerp(velocityAngle, carDirAngle, forwardLerpValue);

				Vector3 lerpDir = new Vector3(Mathf.Cos(lerpAngle), 0, Mathf.Sin(lerpAngle)).normalized;
				float speed = (cos <= 0 ? -CalculatedSpeed() : CalculatedSpeed());
				float n = Mathf.Abs(1 / Vector3.Dot(transform.forward, lerpDir));

				velocity = lerpDir * speed * n;
			}
			rigid.velocity = velocity;
		}
		else // 감속
		{
			Vector3 dragDir;
			Vector3 n = Vector3.Cross(transform.forward, velocityDir);
			dragDir = Vector3.Cross(transform.forward, n).normalized;
			velocity = rigid.velocity;
			velocity += dragDir * drag;
			Vector3 v = transform.forward * (CalculatedSpeed(true) - beforeSpeed);

			rigid.velocity = velocity + v;

#if UNITY_EDITOR
			Debug.DrawRay(transform.position, dragDir * 50f, Color.magenta); // Drag의 방향
		}

		Debug.DrawRay(transform.position, desireDir * 50f, Color.red); // 핸들 방향
		Debug.DrawRay(transform.position, velocityDir * rigid.velocity.magnitude, Color.green); // 힘의 방향
		Debug.DrawRay(transform.position, transform.forward * beforeSpeed, Color.blue); // 객체 방향
#else
		}
#endif
	}

	private float CalculatedSpeed(bool isDragging = false)
	{
		float angleBetween = Vector3.Angle(transform.forward, rigid.velocity.normalized);
		float cosBetween = Mathf.Cos(angleBetween * Mathf.Deg2Rad);
		float speedForward = cosBetween * rigid.velocity.magnitude;
		beforeSpeed = speedForward;
		float calcSpeed = beforeSpeed;

		if (calcSpeed <= maxSpeed)
		{
			if (isDragging)
			{
				calcSpeed += acceleration * dragAcceleration * Time.fixedDeltaTime;
			}
			else
			{
				calcSpeed += acceleration * Time.fixedDeltaTime;
			}
			calcSpeed = Mathf.Clamp(calcSpeed, float.MinValue, maxSpeed);
		}

		return calcSpeed;
	}

	public void SetMove(bool enable)
	{
		move = enable;
		if (!move) StopMovement();
	}

	public void StopMovement()
	{
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
	}
	#endregion

	#region Ground
	private void CheckGround()
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo, groundRayDistance);
		if (isHit && hitInfo.distance <= (1f - gravityVelocity * Time.fixedDeltaTime))
		{
			gravityVelocity = 0f;
			transform.position = new Vector3(transform.position.x, hitInfo.point.y + 1f, transform.position.z);
		}
		else
		{
			gravityVelocity += gravity * Time.fixedDeltaTime;
			transform.position += Vector3.one * gravityVelocity * Time.fixedDeltaTime;
		}
	}
	#endregion

	#region Turn
	public void SetAngleDesire(float angleInput)
	{
		inputAngle = angleInput;
	}

	private void Turn()
	{
		float rotY = transform.eulerAngles.y;
		rotY = rotY > 180 ? rotY - 360f : rotY;
		float delta = Mathf.DeltaAngle(rotY, inputAngle);
		float sign = Mathf.Sign(delta);
		float linearAngle = rotY + sign * turnRate * Time.fixedDeltaTime;
		if (sign != Mathf.Sign(inputAngle - linearAngle) && Mathf.Abs(Mathf.DeltaAngle(rotY, inputAngle)) < 1f)
		{
			linearAngle = inputAngle;
		}

		transform.rotation = Quaternion.Euler(0, linearAngle, 0);
	}
	#endregion

	#region Collision
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Wall"))
		{
			ContactPoint point = collision.contacts[0];
			float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, -point.normal, Vector3.up)) * Mathf.Deg2Rad;
			float normalVelocity = Mathf.Cos(angle) * collision.relativeVelocity.magnitude;
			if (normalVelocity > bounceSpeed)
			{
				rigid.AddForce(point.normal * bouncePower, ForceMode.Impulse);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Target"))
		{
			OnTriggerCheckPoint?.Invoke(this, other.gameObject);
		}
	}
	#endregion
}
