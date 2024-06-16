using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIComponents : MonoSingleton<UIComponents>
{
	private Dictionary<string, UIBehaviour[]> uiComponents;

	private void Awake()
	{
		uiComponents = new Dictionary<string, UIBehaviour[]>();

		Image[] images = GetComponentsInChildren<Image>();
		uiComponents.Add("Image", images);
		TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
		uiComponents.Add("TextMeshProUGUI", texts);
		Button[] buttons = GetComponentsInChildren<Button>();
		uiComponents.Add("Button", buttons);
	}

	public T FindUIElement<T>(string name) where T : UIBehaviour
	{
		if (uiComponents.TryGetValue(typeof(T).Name, out UIBehaviour[] arr))
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
}
