using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
	public GameObject PlayerVisual { get; set; }
	[HideInInspector] public int selectedCarIndex = 0;

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
