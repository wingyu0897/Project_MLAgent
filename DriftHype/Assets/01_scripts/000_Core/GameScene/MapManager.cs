using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<Map> maps;
	public CarController playerCar;
	public CarController aiCar;
	public FollowCam followCam;

	private Map map;

	private bool isRunning = false;

	private void Start()
	{
		SetupGame();
		GameSceneManager.Instance.OnStateChanged += CheckState;
	}

	private void SetupGame()
	{
		playerCar = Instantiate(playerCar);
		aiCar = Instantiate(aiCar);
		map = Instantiate(maps[Random.Range(0, maps.Count)]);
		UIComponents.Instance.GetUIElement<TextMeshProUGUI>("MapName")?.SetText(map.gameObject.name.Replace("(Clone)", ""));

		map.Initialize(playerCar, aiCar);
		followCam.target = playerCar.transform;
	}

	private void CheckState(GAME_STATE state)
	{
		if (state == GAME_STATE.RUNNING)
		{
			StartCounting();
		}
		else if (state == GAME_STATE.END)
		{
			EndGame();
		}
	}

	#region start
	private void StartCounting()
	{
		StartCoroutine(BeginCounting());
	}

	private void StartGame()
	{
		playerCar.SetMove(true);
		playerCar.input?.EnableInput();
		aiCar.SetMove(true);
		aiCar.input?.EnableInput();

		isRunning = true;
	}

	private void Update()
	{
		if (isRunning)
			GameSceneManager.Instance.RacingTime += Time.deltaTime;
	}

	private IEnumerator BeginCounting()
	{
		for (int i = 3; i > 0; i--)
		{
			map.SetCountText(i.ToString());
			yield return new WaitForSeconds(1f);
		}
		map.SetCountText("GO!");
		StartGame();
		yield return null;
	}
	#endregion

	#region End
	private void EndGame()
	{
		playerCar.SetMove(false);
		playerCar.input?.DisableInput();
		aiCar.SetMove(false);
		aiCar.input?.DisableInput();

		isRunning = false;
	}
	#endregion
}

