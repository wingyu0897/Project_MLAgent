using System;
using UnityEngine;

public interface ICar
{
	public GameObject nextTarget { get; set; }

	public event Action<ICar, GameObject> OnTriggerCheckPoint;
}
