﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;


public class FightResultDialog : MonoBehaviour
{

	public GameObject fightResultPanelObject;
	public Text actionDescription;
	public Button okButton;
	public Text scoreUser;

	public GameObject imageWin;
	public GameObject imageLose;
	public GameObject imageDraft;

	public GameObject	rightAnswerObject;
	public Text			answerDescription;

	public List<Image>	fightsImageList;

	public void SetText (string text, int fightState, UnityAction okEvent, List<Fight> fight, string scrUs, string pRightAnswer)
	{

		Sprite spriteDraft		= null;
		Sprite spriteWin		= null;
		Sprite spriteLose		= null;
		Sprite spriteDefault	= null;

		Texture2D texture = Resources.Load ("draft_sign") as Texture2D;
		if (texture != null) {
			spriteDraft = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("win_sign") as Texture2D;
		if (texture != null) {
			spriteWin = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("lose_sign") as Texture2D;
		if (texture != null) {
			spriteLose = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		if (spriteDefault == null) {
			texture = Resources.Load ("default_sign") as Texture2D;
			if (texture != null) {
				spriteDefault = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		fightsImageList [0].gameObject.SetActive (true);
		fightsImageList [1].gameObject.SetActive (true);
		fightsImageList [2].gameObject.SetActive (true);

		foreach (var cc in fightsImageList) {
			cc.sprite = spriteDefault;
		}

		fightResultPanelObject.SetActive (true);

		if (fight.Count > 1) {
			foreach (var currentFight in fight) {
				if (currentFight.IsDraw == true) {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteDraft;
				} else if (currentFight.Winner == UserController.currentUser.Id) {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteWin;
				} else {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteLose;
				}
			}
		} else {
			
			fightsImageList [0].gameObject.SetActive (false);
			fightsImageList [2].gameObject.SetActive (false);

			if (fight[0].IsDraw == true) {
				fightsImageList [1].sprite = spriteDraft;
			} else if (fight[0].Winner == UserController.currentUser.Id) {
				fightsImageList [1].sprite = spriteWin;
			} else {
				fightsImageList [1].sprite = spriteLose;
			}
		}

		actionDescription.text = System.Text.RegularExpressions.Regex.Unescape (text);
		scoreUser.text = scrUs;

		if (fightState == -1) {
			imageLose.SetActive (true);
		} else if (fightState == 0) {
			imageDraft.SetActive (true);
		} else {
			imageWin.SetActive (true);
		}

		okButton.onClick.RemoveAllListeners ();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		okButton.gameObject.SetActive (true);

		//Show right answer
		if(/*StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE &&*/ fight.Count == 1 && 
			(fight[fight.Count-1].FightTypeId == 1 || fight[fight.Count-1].FightTypeId == 2 || fight[fight.Count-1].FightTypeId == 3 
				|| fight[fight.Count-1].FightTypeId == 5)){

			rightAnswerObject.SetActive (true);
			answerDescription.text = pRightAnswer;
		}
	}

	public void ClosePanel ()
	{
		rightAnswerObject.SetActive (false);
		imageLose.SetActive (false);
		imageDraft.SetActive (false);
		imageWin.SetActive (false);

		fightResultPanelObject.SetActive (false);

	}
}
