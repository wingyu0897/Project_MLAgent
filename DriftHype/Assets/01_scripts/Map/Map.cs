using System;
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

	[HideInInspector] public bool IsAITraining = false;

	public event Action OnGameEnd;
	public event Action<ICar, bool> IsRightCheckPoint;

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
			cars[i].transform.position = startPoint.position + startPoint.right * (area + i * DistanceBetweenCars);
			cars[i].transform.rotation = Quaternion.Euler(0, startPoint.eulerAngles.y, 0);

			cars[i].SetNextTarget(checkPoints[0]);
			cars[i].OnTriggerCheckPoint += SetPoint;
			this.cars.Add(cars[i]);
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < cars.Count; ++i)
		{
			cars[i].OnTriggerCheckPoint -= SetPoint;
		}
	}

	private void SetPoint(ICar car, GameObject point)
	{
		bool isRightPoint = car.NextTarget.Equals(point);
		IsRightCheckPoint?.Invoke(car, isRightPoint);
		if (isRightPoint)
		{
			if (point == checkPoints[checkPoints.Count - 1]) // 마지막 체크포인트에 도달했다면 게임 종료
			{
				if (IsAITraining || (GameSceneManager.Instance is not null && GameSceneManager.Instance.CurrentState != GAME_STATE.END))
				{
					OnGameEnd?.Invoke();
					GameSceneManager.Instance.IsPlayerWin = car.IsPlayer;
					GameSceneManager.Instance.ChangeState(GAME_STATE.END);
				}
			}
			else
			{
				GameObject nextTarget = checkPoints[checkPoints.IndexOf(point) + 1];
				car.SetNextTarget(nextTarget);
			}
		}
	}

	public void SetCountText(string txt)
	{
		countText.text = txt;
	}
}
