using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

    [Header("Test")]
    [SerializeField] private Map testMap;
    [SerializeField] private CarController car;

	private void Awake()
	{
		Instance = this;
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
