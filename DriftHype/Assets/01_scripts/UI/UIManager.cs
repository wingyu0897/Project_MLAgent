using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private void Start()
	{
		UIComponents.Instance.FindUIElement<Button>("StartButton")?.onClick.AddListener(GameManager.Instance.StartGame);
	}
}
