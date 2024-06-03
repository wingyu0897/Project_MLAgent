using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<GameObject> checkPoints;
	private Transform startPoint;

	private List<CarController> cars = new List<CarController>();

	private void Awake()
	{
		startPoint = transform.Find("StartPoint");

		checkPoints = new List<GameObject>();
		Transform checkPointParent = transform.Find("CheckPoints");
		if (checkPointParent)
		{
			for (int i = 0; i < checkPointParent.childCount; ++i)
			{
				checkPoints.Add(checkPointParent.GetChild(i).gameObject);
			}
		}
	}

	public void Initialize(params CarController[] cars)
	{

		foreach (CarController car in cars)
		{
			car.transform.position = startPoint.position + Vector3.up;
			car.nextTarget = checkPoints[0];
			car.OnTriggerCheckPoint += SetPoint;
			this.cars.Add(car);
		}
	}

	private void SetPoint(ICar car, GameObject point)
	{
		if (car.nextTarget.Equals(point))
		{
			print("Right");
			if (point == checkPoints[checkPoints.Count - 1])
			{
				foreach (CarController elem in cars)
				{
					elem.SetMove(false);
					elem.input.DisableInput();
				}
				print("End");
			}
			else
			{
				GameObject nextTarget = checkPoints[checkPoints.IndexOf(point) + 1];
				car.nextTarget = nextTarget;
			}
		}
	}

	private void EndGame()
	{
		foreach (ICar car in cars)
		{
			car.nextTarget = null;
		}
	}
}
