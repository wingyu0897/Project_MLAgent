using System.Collections.Generic;
using UnityEngine;

public class AIMapManager : MonoBehaviour
{
    public List<Map> maps;
    private Map map;
    
    public Map GenerateMap(CarController aiCar)
	{
        if (map != null)
		{
            Destroy(map.gameObject);
		}

        map = Instantiate(maps[Random.Range(0, maps.Count)], transform.position, Quaternion.identity);
        map.IsAITraining = true;
        map.transform.SetParent(transform);
        map.Initialize(aiCar);
        return map;
    }
}
