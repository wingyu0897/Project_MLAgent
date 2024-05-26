using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInputAI : Agent
{
	private CarController carController;
	private float currentAngle;

	[Header("Controll")]
	[SerializeField] private float handleSensitive = 360f;

	[Header("Navigate")]
	[SerializeField] private Transform target;

	public override void Initialize()
	{
		carController = GetComponent<CarController>();
	}

	public override void OnEpisodeBegin()
	{
		currentAngle = 0f;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(Vector3.Distance(transform.position, target.position));
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

		AddReward(1f / StepCount);

		if (Vector3.Distance(transform.position, target.position) <= 0.1f)
		{
			AddReward(1f);
			EndEpisode();
		}
	}
}
