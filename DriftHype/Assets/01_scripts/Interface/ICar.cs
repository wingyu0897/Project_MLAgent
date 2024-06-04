using System;
using UnityEngine;

public interface ICar
{
	public GameObject NextTarget { get; set; }
	public bool IsPlayer { get; set; }

	public event Action<ICar, GameObject> OnTriggerCheckPoint;
}
