using System;
using UnityEngine;

public interface ICar
{
	public GameObject NextTarget { get; set; }
	public bool IsPlayer { get; set; }

	public void SetNextTarget(GameObject nextTarget);

	public event Action<ICar, GameObject> OnTriggerCheckPoint;
}
