using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIComponents : MonoSingleton<UIComponents>
{
	private Dictionary<string, UIBehaviour[]> _uiComponents;
	private Dictionary<string, Component[]> _components;

	private void Awake()
	{
		_uiComponents = new Dictionary<string, UIBehaviour[]>();
		_components = new Dictionary<string, Component[]>();

		FindUIComponents<Image>();
		FindUIComponents<TextMeshProUGUI>();
		FindUIComponents<Button>();
		FindUIComponents<VerticalLayoutGroup>();
		FindUIComponents<Scrollbar>();

		FindNonUIComponents<Transform>();
		FindNonUIComponents<CanvasGroup>();
	}

	private void FindUIComponents<T>() where T : UIBehaviour
	{
		T[] components = GetComponentsInChildren<T>(true);
		_uiComponents.Add(typeof(T).Name, components);
	}

	private void FindNonUIComponents<T>() where T : Component
	{
		T[] components = GetComponentsInChildren<T>(true);
		_components.Add(typeof(T).Name, components);
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

	public T GetNonUIElement<T>(string name) where T : Component
	{
		if (_components.TryGetValue(typeof(T).Name, out Component[] arr))
		{
			foreach (Component elem in arr)
			{
				if (elem.gameObject.name.Equals(name))
				{
					return elem as T;
				}
			}
		}

		return null;
	}
}
