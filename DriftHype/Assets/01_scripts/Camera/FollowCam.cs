using UnityEngine;

[ExecuteInEditMode]
public class FollowCam : MonoBehaviour
{
    [SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;

	private void FixedUpdate()
	{
		if (target != null)
			transform.position = Vector3.Lerp(transform.position, (target.position + offset), 0.2f);
	}
}
