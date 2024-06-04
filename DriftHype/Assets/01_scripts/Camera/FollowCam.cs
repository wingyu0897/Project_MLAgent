using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
	[SerializeField] private Vector3 offset;

	private void Start()
	{
		if (target != null)
			transform.position = target.position + offset;
	}

	private void FixedUpdate()
	{
		if (target != null)
			transform.position = Vector3.Lerp(transform.position, (target.position + offset), 0.2f);
	}
}
