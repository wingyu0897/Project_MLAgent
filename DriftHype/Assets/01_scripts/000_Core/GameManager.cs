using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
	[SerializeField] private GameObject playerVisual;
	public GameObject PlayerVisual => playerVisual;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene("Menu");
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void EndGame()
	{
		SceneManager.LoadScene("Menu");
	}
}
