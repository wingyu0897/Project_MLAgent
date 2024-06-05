using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    private Dictionary<GAME_STATE, CanvasGroup> canvas = new();

	[SerializeField] private Button startBtn;
	[SerializeField] private Button menuBtn;
	[SerializeField] private TextMeshProUGUI winnerTxt;
	[SerializeField] private TextMeshProUGUI raceTimeTxt;

	private void Awake()
	{
		var comps = GetComponentsInChildren<CanvasGroup>();
		foreach (GAME_STATE state in Enum.GetValues(typeof(GAME_STATE)))
		{
			string str = state.ToString();
			str = char.ToUpper(str[0]) + str.ToLower().Substring(1);
			CanvasGroup cG = transform.Find(str).GetComponent<CanvasGroup>();
			canvas.Add(state, cG);
		}

		GameSceneManager.Instance.OnStateChanged += SetUIByState;

		startBtn?.onClick.AddListener(() => GameSceneManager.Instance.ChangeState(GAME_STATE.RUNNING));
		menuBtn?.onClick.AddListener(GameManager.Instance.EndGame);
	}

	public void SetUIByState(GAME_STATE state)
	{
		foreach (var elem in canvas)
		{
			SetVisibleCanvasGroup(elem.Value, false);
		}
		SetVisibleCanvasGroup(canvas[state], true);

		if (state == GAME_STATE.END)
		{
			EndUI();
		}
	}

	private void EndUI()
	{
		winnerTxt.text = GameSceneManager.Instance.isPlayerWin ? "You Win" : "AI Win";
		raceTimeTxt.text = String.Format("{0:f2}s", GameSceneManager.Instance.racingTime);
	}

	private void SetVisibleCanvasGroup(CanvasGroup cg, bool visible)
	{
		cg.alpha = visible ? 1f : 0f;
		cg.blocksRaycasts = visible;
		cg.interactable = visible;
	}
}
