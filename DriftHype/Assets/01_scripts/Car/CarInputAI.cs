using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInputAI : Agent
{
	private CarController carController;
	private float currentAngle;
	[SerializeField] RandomMapGenerator map;

	[Header("Controll")]
	[SerializeField] private float handleSensitive = 360f;

	[Header("Navigate")]
	[SerializeField] private Transform target;

	[SerializeField] private MeshRenderer meshRen;
	private Material originMat;
	[SerializeField] private Material successMat;

	public override void Initialize()
	{
		carController = GetComponent<CarController>();
		originMat = meshRen.material;
	}

	public override void OnEpisodeBegin()
	{
		map.GenerateMap();
		currentAngle = 0f;
		transform.localPosition = Vector3.up;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(Vector3.Distance(transform.position, target.position));
		sensor.AddObservation(Vector3.Dot((target.position - transform.position).normalized, transform.forward));
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		var DiscreteActions = actions.DiscreteActions;
		int angle = DiscreteActions[0];

		currentAngle = angle switch
		{
			1 => currentAngle -= handleSensitive * Time.fixedDeltaTime,
			2 => currentAngle += handleSensitive * Time.fixedDeltaTime,
			_ => currentAngle,
		};

		carController.SetAngleDesire(currentAngle);

		AddReward(1f / StepCount);

		if (Vector3.Distance(transform.position, target.position) <= 15f)
		{
			StartCoroutine(ChangeFloorColorCo());
			AddReward(10f);
			EndEpisode();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Wall"))
		{
			AddReward(-0.1f);
		}
	}

	private IEnumerator ChangeFloorColorCo()
	{
		meshRen.material = successMat;
		yield return new WaitForSeconds(0.1f);
		meshRen.material = originMat;
	}
}
