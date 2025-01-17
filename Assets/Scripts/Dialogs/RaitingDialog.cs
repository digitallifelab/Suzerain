﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class RaitingDialog : MonoBehaviour {

	public GameObject 	ratingPanelObject;
	public Button 		okButton;
	public Text 		headLine;
	public Text 		playersCount;

	List<Friend> 					mFriends;
	List<FriendTemplateButton> 		friendsButtons;

	public	GameObject				friendButton;
	public	Transform				ListFriendsPanel;


	public void ShowDialog(List<Friend> pFriends, UnityAction okEvent,string header, int pPlayersCount ){

		ClearList ();

		mFriends = pFriends;

		InitSearchList ();

		headLine.text = header;
		playersCount.text = pPlayersCount.ToString ();


		ratingPanelObject.SetActive (true);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);

		okButton.gameObject.SetActive (true);
	}

	private void InitSearchList(){

		friendsButtons			= new List<FriendTemplateButton>();

		//Bottom array
		foreach (var c in mFriends) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(friendButton) as GameObject;
			FriendTemplateButton button1 = newButtonItem.GetComponent<FriendTemplateButton>();

			if (c.UserId == UserController.currentUser.Id) {
				button1.backGroundMe.gameObject.SetActive (true);
			}

			button1.number.text = c.State.ToString();
			button1.friend = c;

			button1.NameUser.text = c.UserName;
			button1.sCore.text = c.Score.ToString();

			var shield = Utility.getShieldNumByScore (c.Score);
			if(shield.shieldNumber != "1")
				button1.rankUser.text = ScreensManager.LMan.getString(Utility.getRunkByNumber (c.Rank));

			//if (c.Rank != -1) {
				Utility.setAvatarByState (button1.shieldImage, c.Score);
			//}
			//else
			//	Utility.setAvatarByState (button1.shieldImage, -1);

			button1.button.onClick.RemoveAllListeners();
			//button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );

			newButtonItem.transform.SetParent(ListFriendsPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			friendsButtons.Add (button1);
		}
	}

	public void ClosePanel () {
		ClearList ();
		ratingPanelObject.SetActive (false);
	}

	protected void ClearList(){

		foreach (Transform child in ListFriendsPanel) {
			GameObject.Destroy(child.gameObject);
		}
	}

}
