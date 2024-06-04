using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<GameObject> checkPoints;
	private Transform startPoint;

	private List<CarController> cars = new List<CarController>();
	[SerializeField] private TextMeshPro countText;

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
				GameSceneManager.Instance.ChangeState(GAME_STATE.END);
				if (car.IsPlayer)
				{
					GameSceneManager.Instance.isPlayerWin = true;
				}
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

	private void EndGame()
	{
		foreach (ICar car in cars)
		{
			car.NextTarget = null;
		}
	}
}
