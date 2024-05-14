using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Value Setting")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
	[SerializeField] private float acceleration;
    [SerializeField] private float turnRate;

    [Header("References")]
    private Rigidbody rigid;

	[Header("Flags")]
	private float turnDesire;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.D))
		{
			Turn(1f);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			Turn(-1f);
		}
	}

	private void FixedUpdate()
	{
		rigid.AddForce(transform.forward * moveSpeed * Time.fixedDeltaTime);
		Vector3 velocity = rigid.velocity;
		velocity.z = 0;
		if (velocity.magnitude > maxSpeed)
		{
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
			rigid.velocity = velocity;
		}
	}

	private void Turn(float direction)
	{
		transform.Rotate(Vector3.up, direction * turnRate * Time.deltaTime);
	}
}
