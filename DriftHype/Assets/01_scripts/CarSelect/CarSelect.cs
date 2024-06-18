using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelect : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform _camTrm;
	[SerializeField] private GameObject uiPrefab;
	[SerializeField] private List<GameObject> cars;
	private Scrollbar _scrollbar;

	[Header("Value Settings")]
	[SerializeField] private float interpol = 15f;
	[SerializeField] private float selectedScale = 2f;
	[SerializeField][Range(0, 1f)] private float scaleTime = 0.5f;
	[Tooltip("스크롤을 당겼을 때, 미끄러지지 않고 멈추는 비율")]
	[SerializeField] private float swipeSlidingMaxinum = 0.01f;

	private Transform[] _carVisuals;

	private float _scrollPos = 1f;
	private float _prevScrollPos;
	private float _distance;
	private float[] _pos;
	private bool isLerping = true;

	private int _currentSelectIndex;
	private Coroutine[] _sizeCoroutines;

	private void Start()
	{
		_scrollbar = UIComponents.Instance.GetUIElement<Scrollbar>("Scrollbar Vertical");
		_sizeCoroutines = new Coroutine[cars.Count];
		_carVisuals = new Transform[cars.Count];

		_pos = new float[cars.Count];
		_distance = 1f / (cars.Count - 1);
		for (int i = 0; i < cars.Count; ++i)
			_pos[cars.Count - 1 - i] = _distance * i;

		RectTransform content = UIComponents.Instance.GetNonUIElement<Transform>("Content").GetComponent<RectTransform>();
		Transform carParent = transform.GetChild(0);
		for (int i = 0; i < cars.Count; ++i)
		{
			GameObject carVisual = Instantiate(cars[i], -Vector3.up * (interpol * i), Quaternion.identity, carParent); 
			RectTransform ui = new GameObject("Car", typeof(RectTransform)).GetComponent<RectTransform>();
			ui.sizeDelta = Vector2.zero;
			ui.transform.SetParent(content);
			ui.transform.localPosition = Vector3.zero;
			_carVisuals[i] = carVisual.transform;
		}

		_currentSelectIndex = GameManager.Instance.selectedCarIndex;
		_scrollbar.value = _pos[_currentSelectIndex];
		_scrollPos = _scrollbar.value;
		_prevScrollPos = _scrollPos;
		content.localPosition += Vector3.up * 500f * _currentSelectIndex;

		SetSize(_currentSelectIndex, true);
		GameManager.Instance.PlayerVisual = cars[_currentSelectIndex];
	}

	private void Update()
	{
		if (Input.touchCount > 0 || Input.GetMouseButton(0))
		{
			isLerping = false;
			_scrollPos = _scrollbar.value;
		}
		else if ((Mathf.Abs(_prevScrollPos - _scrollbar.value) <= swipeSlidingMaxinum * Time.deltaTime) || isLerping)
		{
			isLerping = true;
			for (int i = 0; i < cars.Count; ++i)
			{
				if (_scrollPos < _pos[i] + (_distance * 0.5f) && _scrollPos > _pos[i] - (_distance * 0.5f))
				{
					if (_currentSelectIndex != i)
					{
						SetSize(_currentSelectIndex, false);

						_currentSelectIndex = i;
						GameManager.Instance.PlayerVisual = cars[_currentSelectIndex];
						GameManager.Instance.selectedCarIndex = _currentSelectIndex;

						SetSize(_currentSelectIndex, true);
					}
					_scrollbar.value = Mathf.Lerp(_scrollbar.value, _pos[i], 0.03f);
					break;
				}
			}
		}
		else
		{
			_scrollPos = _scrollbar.value;
		}
		_prevScrollPos = _scrollbar.value;
		_camTrm.position = -Vector3.up * interpol * (1 - _scrollbar.value) * (cars.Count - 1);
	}
	
	private void SetSize(int index, bool isBigger)
	{
		if (_sizeCoroutines[index] != null)
			StopCoroutine(_sizeCoroutines[index]);

		_sizeCoroutines[index] = StartCoroutine(SetSizeCo(_carVisuals[index], isBigger));

	}

	private IEnumerator SetSizeCo(Transform target, bool isBigger)
	{
		float value = 0f;
		float mul = 1f / scaleTime;
		float size = isBigger ? selectedScale : 1f;
		Vector3 originSize = target.localScale;
		while (value < 1f) // 1초간 사이즈 변경
		{
			target.localScale = Vector3.Lerp(originSize, Vector3.one * size, value);

			value += Time.deltaTime * mul;
			yield return null;
		}
	}
}