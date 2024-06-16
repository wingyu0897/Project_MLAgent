using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIComponents : MonoSingleton<UIComponents>
{
	private Dictionary<string, UIBehaviour[]> _uiComponents;
	private Transform[] _objects;

	private void Awake()
	{
		_objects = GetComponentsInChildren<Transform>(true);

		_uiComponents = new Dictionary<string, UIBehaviour[]>();

		GetUIComponents<Image>();
		GetUIComponents<TextMeshProUGUI>();
		GetUIComponents<Button>();
		GetUIComponents<VerticalLayoutGroup>();
		GetUIComponents<Scrollbar>();
	}

	private void GetUIComponents<T>() where T : UIBehaviour
	{
		T[] components = GetComponentsInChildren<T>(true);
		_uiComponents.Add(typeof(T).Name, components);
	}

	public T GetUIElement<T>(string name) where T : UIBehaviour
	{
		if (_uiComponents.TryGetValue(typeof(T).Name, out UIBehaviour[] arr))
		{
			foreach (UIBehaviour elem in arr)
			{
				if (elem.gameObject.name.Equals(name))
				{
					return elem as T; 
				}
			}
		}

		return null;
	}

	public GameObject GetObject(string name)
	{
		foreach (Transform obj in _objects)
		{
			if (obj.gameObject.name.Equals(name))
			{
				return obj.gameObject;
			}
		}

		return null;
	}
}
