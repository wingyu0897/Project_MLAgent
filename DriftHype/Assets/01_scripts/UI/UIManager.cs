using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private GameObject _menuPanel;
	private GameObject _carsPanel;

	private void Start()
	{
		_menuPanel = UIComponents.Instance.GetComponent<Transform>("MenuPanel").gameObject;
		_carsPanel = UIComponents.Instance.GetComponent<Transform>("CarsPanel").gameObject;

		SettingUI();
	}

	private void SettingUI()
	{
		UIComponents instance = UIComponents.Instance;

		float n = 800f / Screen.width; // ĵ���� ȭ�� ����
		int paddingHeight = Mathf.RoundToInt(Screen.height * n * 0.5f - 50); // ĵ���� ����(ȭ�� ���� * ĵ���� ����) / 2 - 50(�е�)
																			 // Content�� �е��� ������ ���� ������Ʈ���� �߾ӿ� ���ĵ� �� �ֵ��� ������
		instance.GetUIElement<VerticalLayoutGroup>("Content").padding = new RectOffset(0, 0, paddingHeight, paddingHeight);

		// CarsButton�� ������ ���� ���� �г� Ȱ��ȭ
		instance.GetUIElement<Button>("CarsButton")?.onClick.AddListener(() =>
		{
			_menuPanel.SetActive(false);
			_carsPanel.SetActive(true);
		});

		// StartButton�� ������ ���� ����
		instance.GetUIElement<Button>("StartButton")?.onClick.AddListener(GameManager.Instance.StartGame);

		// CloseCarsPanel�� ������ ���� ���� �г� ��Ȱ��ȭ
		instance.GetUIElement<Button>("CloseCarsPanel")?.onClick.AddListener(() =>
		{
			_menuPanel.SetActive(true);
			_carsPanel.SetActive(false);
		});
	}
}
