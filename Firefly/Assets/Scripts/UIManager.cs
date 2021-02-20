using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Const texts
	private const string ENGAME_TEXT = "You and your colony have safely escaped the clench of darkness.\nAs you move toward the blue light you rest, \nknowing that you are bringing along with yourself the gift of knowledge";
	private const string REPLAY_PROMPT = "You and your colony have safely escaped the clench of darkness.\nAs you move toward the blue light you start thinking...\n\nWas there more to discover?";

	private const string SHUTTLE_TEXT = "This looks like an old space shuttle humans used when they first colonized Mars. \nIt has seen better days.\nYou wonder if you can find more objects of interest like this in the cave.";
	private const string HELMET_TEXT = "Ah! A cracked astronaut helmet. \nKnowing that humans couldn't breathe without enough oxygen, you become remorseful for the fallen human, and realize the importance of your colony.";
	private const string POTATO_TEXT = "What is this weird substance? \nYou have never seen anything like this before. \nIt looks like a brown soggy piece of fruit. \nIt almost looks like somebody put this here for a pop-culture reference.";
	private const string PARCHMENT_TEXT = "Ah! The glorious days of the Pyralion Empire! How you long to hear a bard's song. \nYou should find a way out of this cave before your colony becomes restless.";
	private const string STONE_TEXT = "Is that... <i>supposed</i> to be glowing?";
	#endregion

	[SerializeField] private GameObject endgamePanel;
	[SerializeField] private TextMeshProUGUI endgameText;
	[SerializeField] private TextMeshProUGUI infoText;
	[SerializeField] private GameObject lorePanel;
	[SerializeField] private TextMeshProUGUI loreText;
	[SerializeField] private Image buttonImage;
	[SerializeField] private TextMeshProUGUI restartText;

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

	public void ActivateEndgameWindow()
	{
		Cursor.visible = true;
		StartCoroutine(EndgameFadeOut());	
	}

	private IEnumerator EndgameFadeOut()
	{
		var unseenLore = 0;
		var panelImage = endgamePanel.GetComponent<Image>();
		foreach (var item in LoreManager.LoreSeen)
		{
			if (!item)
			{
				unseenLore++;
			}
		}

		endgamePanel.SetActive(true);

		if (unseenLore == 0)
		{
			endgameText.text = ENGAME_TEXT;
			infoText.text = "You have discovered everything, thanks for playing :)";
		}
		else
		{
			endgameText.text = REPLAY_PROMPT;
			infoText.text = $"(You have {unseenLore} undiscovered secrets.)";
		}

		var timer = 0f;
		while (timer < 1f)
		{
			timer += Time.deltaTime / 2;

			panelImage.color = new Color(0, 0, 0, timer);
			yield return null;
		}
		timer = 0f;

		var endgameColor = endgameText.color;
		var buttonColor = buttonImage.color;
		var restartColor = restartText.color;
		var infoColor = infoText.color;
		while (timer < 1f)
		{
			timer += Time.deltaTime;

			endgameText.color = new Color(endgameColor.r, endgameColor.g, endgameColor.b, timer);
			buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, timer);
			restartText.color = new Color(restartColor.r, restartColor.g, restartColor.b, timer);
			infoText.color = new Color(infoColor.r, infoColor.g, infoColor.b, timer);
			yield return null;
		}
		Time.timeScale = 0f;
	}

	public void RestartGame()
	{
		Cursor.visible = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
