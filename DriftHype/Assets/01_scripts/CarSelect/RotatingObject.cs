using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Transform rotateTrm;
	[SerializeField] private float rotatingSpeed = 10f;

	private void Update()
	{
		rotateTrm.Rotate(new Vector3(0, rotatingSpeed * Time.deltaTime, 0));
	}
}
