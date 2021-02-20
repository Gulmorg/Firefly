using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	#region Lore texts
	private const string SHUTTLE_TEXT = "Lorem Shuttle";
	private const string HELMET_TEXT = "Lorem helm";
	private const string POTATO_TEXT = "Lorem pot";
	private const string PARCHMENT_TEXT = "Lorem par";
	private const string STONE_TEXT = "Lorem stone";
	#endregion

	[SerializeField] private GameObject endgamePanel;
	[SerializeField] private TextMeshProUGUI endgameText;
	[SerializeField] private GameObject lorePanel;
	[SerializeField] private TextMeshProUGUI loreText;

	public UIManager instance { get; private set; }
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public void RestartGame()
	{
		Cursor.visible = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ActivateEndgameWindow()	// ADD FADE IN AND GAME FREEZE
	{
		Cursor.visible = true;
		var unseenLore = 0;

		foreach (var item in LoreManager.LoreSeen)
		{
			if (!item)
			{
				unseenLore++;
			}
		}

		endgamePanel.SetActive(true);
		

	}

	public void ActivateLore(int i = -1)
	{
		lorePanel.SetActive(true);
		LoreManager.LoreSeen[i] = true;

		switch (i)
		{
			case 0:
				loreText.text = SHUTTLE_TEXT;
				break;
			case 1:
				loreText .text= HELMET_TEXT;
				break;
			case 2:
				loreText.text = POTATO_TEXT;
				break;
			case 3:
				loreText.text = PARCHMENT_TEXT;
				break;
			case 4:
				loreText.text = STONE_TEXT;
				break;
		}
	}

	public void DeactivateLore()
	{
		lorePanel.SetActive(false);
	}
}
