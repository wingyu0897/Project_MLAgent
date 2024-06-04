using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInputPlayer : MonoBehaviour, ICarInput
{
    [SerializeField] private HandleUI handleUI;
	private CarController controller;

	public void DisableInput()
	{
		handleUI.OnAngleInput -= InputAngle;
	}

	public void EnableInput()
	{
		handleUI.OnAngleInput += InputAngle;
	}

	private void Awake() 
	{
		controller = GetComponent<CarController>();
		controller.input = this;
		controller.IsPlayer = true;
		handleUI = FindObjectOfType<HandleUI>();
	}

	private void InputAngle(float angleInput)
	{
		controller.SetAngleDesire(angleInput);
	}
}
