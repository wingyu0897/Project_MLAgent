using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;

	private void Start()
	{
		startBtn.onClick.AddListener(GameManager.Instance.StartGame);
	}
}
