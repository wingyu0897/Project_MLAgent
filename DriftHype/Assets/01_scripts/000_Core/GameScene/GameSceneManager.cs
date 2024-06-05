using System;

public enum GAME_STATE
{
	READY,
	RUNNING,
	END,
}

public class GameSceneManager : MonoSingleton<GameSceneManager>
{
	private GAME_STATE currentState;
	public bool isPlayerWin = false;
	public float racingTime = 0f; 

	public event Action<GAME_STATE> OnStateChanged;

	private void Start()
	{
		ChangeState(GAME_STATE.READY);
	}

	public void ChangeState(GAME_STATE newState)
	{
		currentState = newState;
		OnStateChanged?.Invoke(currentState);
	}
}
