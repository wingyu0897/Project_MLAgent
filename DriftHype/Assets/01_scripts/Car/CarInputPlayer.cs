using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInputPlayer : MonoBehaviour
{
    [SerializeField] private HandleUI handleUI;
	private CarController controller;

	private void Awake() 
	{
		controller = GetComponent<CarController>();

		handleUI.OnAngleInput += InputAngle;
	}

	private void InputAngle(float angleInput)
	{
		controller.SetAngleDesire(angleInput);
	}
}
