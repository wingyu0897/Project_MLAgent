using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
	[SerializeField] private Transform checkPointParent;
	private List<GameObject> checkPoints;
	[SerializeField] private Transform startPoint;

	private List<CarController> cars = new List<CarController>();
	[SerializeField] private TextMeshPro countText;

	private void Awake()
	{
		checkPoints = new List<GameObject>();
		if (checkPointParent)
		{
			for (int i = 0; i < checkPointParent.childCount; ++i)
			{
				checkPoints.Add(checkPointParent.GetChild(i).gameObject);
			}
		}
	}

	const float DistanceBetweenCars = 10f;

	public void Initialize(params CarController[] cars)
	{
		float area = DistanceBetweenCars * (cars.Length - 1);
		area = -area + area / 2f;
		for (int i = 0; i < cars.Length; ++i)
		{
			cars[i].transform.position = startPoint.position + Vector3.up + startPoint.right * (area + i * DistanceBetweenCars);
			cars[i].transform.rotation = Quaternion.Euler(0, startPoint.eulerAngles.y, 0);

			cars[i].NextTarget = checkPoints[0];
			cars[i].OnTriggerCheckPoint += SetPoint;
			this.cars.Add(cars[i]);
		}
	}

	private void SetPoint(ICar car, GameObject point)
	{
		if (car.NextTarget.Equals(point))
		{
			if (point == checkPoints[checkPoints.Count - 1])
			{
				if (car.IsPlayer == true)
				{
					GameSceneManager.Instance.isPlayerWin = true;
				}
				GameSceneManager.Instance.ChangeState(GAME_STATE.END);
			}
			else
			{
				GameObject nextTarget = checkPoints[checkPoints.IndexOf(point) + 1];
				car.NextTarget = nextTarget;
			}
		}
	}

	public void SetCountText(string txt)
	{
		countText.text = txt;
	}
}
