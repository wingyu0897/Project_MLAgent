using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInputAI : Agent, ICarInput
{
	[SerializeField] AIMapManager mapManager;

	private CarController carController;
	private float currentAngle;

	[Header("Controll")]
	[SerializeField] private float handleSensitive = 360f;

	public override void Initialize()
	{
		carController = GetComponent<CarController>();
		carController.input = this;
	}

	private void OnTriggerCheck(ICar car, GameObject checkPoint)
	{
		AddReward(1f);
	}

	public override void OnEpisodeBegin()
	{
		carController.StopMovement();
		carController.transform.rotation = Quaternion.Euler(0, 0, 0);
		carController.SetMove(true);
		currentAngle = 0f;
		Map generatedMap = mapManager.GenerateMap(carController);
		generatedMap.OnGameEnd += () =>
		{
			AddReward(100f);
			EndEpisode();
		};
		generatedMap.IsRightCheckPoint += (car, isRight) => AddReward((isRight ? 10f : -10f));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(Vector3.Distance(transform.position, carController.nextTargetTrm.position));
		sensor.AddObservation(Vector3.Dot((carController.nextTargetTrm.position - transform.position).normalized, transform.forward));
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

		if (angle > 0)
		{
			AddReward(-0.01f);
		}


		carController.SetAngleDesire(currentAngle);

		AddReward(-0.01f);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Wall"))
		{
			AddReward(-0.1f);
		}
	}

	public void EnableInput()
	{
		//
	}

	public void DisableInput()
	{
		//
	}
}
