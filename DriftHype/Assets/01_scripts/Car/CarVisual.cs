using System.Collections.Generic;
using UnityEngine;

public class Visual
{
	public GameObject car;
	public List<Transform> frontWheels;
}


public class CarVisual : MonoBehaviour
{
    protected Visual visual;

    public void SetVisual(GameObject car)
	{
		GameObject instance = Instantiate(car, transform);
		instance.name = "Visual";
		instance.transform.localRotation = Quaternion.Euler(0, -90f, 0);

		visual = new Visual();
		visual.car = instance;
		visual.frontWheels = new List<Transform>();
		for (int i = 0; i < instance.transform.GetChild(0).childCount; ++i)
		{
			if (instance.transform.GetChild(0).GetChild(i).name.Equals("FrontWheel"))
			{
				visual.frontWheels.Add(instance.transform.GetChild(0).GetChild(i));
			}
		}
	}

	public void SetFrontWheels(Vector3 direction)
	{
		for (int i = 0; i < visual.frontWheels.Count; ++i)
		{
			visual.frontWheels[i].right = direction;
		}
	}
}
