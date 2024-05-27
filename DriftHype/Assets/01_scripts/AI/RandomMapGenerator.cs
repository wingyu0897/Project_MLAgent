using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private Transform target;
	[SerializeField] private GameObject cubePrefab;

	private List<GameObject> cubes;

	private void Awake()
	{
		cubes = new List<GameObject>();
		for (int i = 0; i < 10; ++i)
		{
			GameObject cube = Instantiate(cubePrefab, transform);
			cubes.Add(cube);
		}
	}

	[ContextMenu("generate")]
	public void GenerateMap()
	{
		//target.localPosition = new Vector3(Random.Range(-200f, 200f), 0, Random.Range(-200f, 200f));

		foreach (GameObject cube in cubes)
		{
			Vector3 pos;
			do
			{
				pos = new Vector3(Random.Range(-200f, 200f), 0, Random.Range(0, 400f));
			} while (Vector3.Distance(pos, target.position) < 70f || Vector3.Distance(pos, transform.position) < 70f);

			cube.transform.localPosition = pos;
		}
	}
}
