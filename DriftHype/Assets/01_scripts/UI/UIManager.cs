using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private GameObject _menuPanel;
	private GameObject _carsPanel;

	private void Start()
	{
		_menuPanel = UIComponents.Instance.GetNonUIElement<Transform>("MenuPanel").gameObject;
		_carsPanel = UIComponents.Instance.GetNonUIElement<Transform>("CarsPanel").gameObject;

		SettingUI();
	}

	private void SettingUI()
	{
		UIComponents instance = UIComponents.Instance;

		float n = 800f / Screen.width; // 캔버스 화질 비율
		int paddingHeight = Mathf.RoundToInt(Screen.height * n * 0.5f - 50); // 캔버스 높이(화면 높이 * 캔버스 비율) / 2 - 50(패딩)
																			 // Content의 패딩을 조절해 하위 오브젝트들이 중앙에 정렬될 수 있도록 변경함
		instance.GetUIElement<VerticalLayoutGroup>("Content").padding = new RectOffset(0, 0, paddingHeight, paddingHeight);

		// StartButton을 누르면 게임 시작
		instance.GetUIElement<Button>("StartButton")?.onClick.AddListener(GameManager.Instance.StartGame);

		// ExitButton을 누르면 게임 종료
		instance.GetUIElement<Button>("ExitButton")?.onClick.AddListener(Application.Quit);

		// CarsButton을 누르면 차량 선택 패널 활성화
		instance.GetUIElement<Button>("CarsButton")?.onClick.AddListener(() =>
		{
			_menuPanel.SetActive(false);
			_carsPanel.SetActive(true);
		});

		// CloseCarsPanel을 누르면 차량 선택 패널 비활성화
		instance.GetUIElement<Button>("CloseCarsPanel")?.onClick.AddListener(() =>
		{
			_menuPanel.SetActive(true);
			_carsPanel.SetActive(false);
		});
	}
}
