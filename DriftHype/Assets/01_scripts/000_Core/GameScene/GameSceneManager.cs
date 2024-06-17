using System;

public enum GAME_STATE
{
	READY,
	RUNNING,
	END,
}

public class GameSceneManager : MonoSingleton<GameSceneManager>
{
	private GAME_STATE _currentState;
	public GAME_STATE CurrentState => _currentState;

	public bool IsPlayerWin = false;
	public float RacingTime = 0f; 

	public event Action<GAME_STATE> OnStateChanged;

	private void Start()
	{
		ChangeState(GAME_STATE.READY);
	}

	public void ChangeState(GAME_STATE newState)
	{
		_currentState = newState;
		OnStateChanged?.Invoke(_currentState);
	}
}
