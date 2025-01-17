﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public delegate void GetLocationSuccessfully();

public class AchivmentsScreenManager : BaseUIClass {

	public Text TextRating;
	public Text TextCount;
	public Text TextNickName;
	public Text ActiveStatus;
	public Text After;
	public Text ScoreText;

	public GameObject GOgrayRing;
	public GameObject GOcolorRing;
	public GameObject GOcaptionRing;

//	ScreensManager screensManager;
//
//	ErrorPanel	errorPanel 	= null;
//	WaitPanel	waitPanel 	= null;

	bool itIsGlobalStatus = true;

	float longitude = 0;
	float lattitude = 0;

	public RaitingDialog ratingDialog;

	public Button ratingButton;

	public Image		CristallShield, shield1, shield2, progressBar;
	public GameObject	cristallShieldObject, shieldProgress;

	void OnEnable(){

		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Achivment screen"));

		StartCoroutine(	RotateRings ());

		screensManager = ScreensManager.instance;

		InitAchivments ();

		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		var currentShield = Utility.getNumberOfShield (Rose.statList);
		bool haveCristallShield = false;

		if (currentShield.position == Utility.shieldsArray.Length) {
			haveCristallShield = true;
		}

		if (haveCristallShield) {
			shieldProgress.SetActive (false);
			cristallShieldObject.SetActive (true);
			Utility.setAvatar (CristallShield, Rose.statList);
		} else {
			cristallShieldObject.SetActive (false);
			shieldProgress.SetActive (true);

			var nextShield = Utility.shieldsArray [currentShield.position];
			Utility.setAvatar (shield1, Rose.statList);
			Utility.setAvatarByState (shield2, nextShield.startScore);

			float koeeff = 100f / (float)(nextShield.startScore - currentShield.startScore);
			float posotion = koeeff * (float)(Rose.statList [0].Score - currentShield.startScore)/100f;
			progressBar.fillAmount = posotion;
			ScoreText.text = Rose.statList [0].Score.ToString () + "/" + nextShield.startScore.ToString ();
		}

	}

	public void InitAchivments(){
	
		if (Rose.statList [0].Fights >= Constants.fightsCount) {
			TextRating.text = Rose.statList [0].GlobalStatus.ToString ();
			ActiveStatus.text = "Глобальный статус";
			After.text = "";
			ratingButton.gameObject.SetActive (true);
		} else {
			After.text = "Через";
			TextRating.text = (Constants.fightsCount - Rose.statList [0].Fights).ToString();
			ActiveStatus.text = "Глобальный статус";
		}

		//ShowStatus ();

		TextCount.text = Rose.statList [0].Fights.ToString();

		TextNickName.text = UserController.UserName;
	}

	public void ShowGlobalStatus(){

		if (Rose.statList [0].Fights >= Constants.fightsCount) {
			if (Rose.statList [0].Fights >= Constants.fightsCount) {
				TextRating.text = Rose.statList [0].GlobalStatus.ToString ();
			} else {
				TextRating.text = "0";
			}
		}
			
		ActiveStatus.text 	= "Глобальный статус";
		itIsGlobalStatus 	= true;
	}

	public void ShowLocalStatus(){


		After.text = "";
		TextRating.text = "0";//Rose.statList [0].LocalStatus.ToString ();

		//if (lattitude == 0) {
			waitPanel = screensManager.ShowWaitDialog ("Определение местоположения");
			StartCoroutine (GetLocation (GetStatisticWithLocation));
		//}


		ActiveStatus.text 	= "Локальный статус";
		itIsGlobalStatus 	= false;
	}

	public void GetStatisticWithLocation(){
		
		UserController usrc = UserController.instance;

		waitPanel = screensManager.ShowWaitDialog ("Получение статистики");
		usrc.LogIn (InitLocalRating, longitude, lattitude);
	}

	public void InitLocalRating(bool error, string source_error, string error_text){

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (error == false) {
			TextRating.text = Rose.statList [0].LocalStatus.ToString ();

		} else {
			if(source_error == Constants.LOGIN_ERROR){				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString ("@connection_error"), finishError);
			}else{
				errorPanel = screensManager.ShowErrorDialog(error_text, finishError);
			}
		}
	}

	public void finishError(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}

	public IEnumerator GetLocation(GetLocationSuccessfully pSuccess)
	{
		string resultText;

		if (!Input.location.isEnabledByUser) {
			resultText = "Disabled by user";
			Debug.Log("Disabled by user");

			screensManager.CloseWaitPanel(waitPanel);
			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@geolocation_doesnt_swith_on"), finishError);

			yield break;
		}

		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			resultText = "Waiting";
			Debug.Log("Waiting");
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			resultText = "Timed out";
			Debug.Log("Timed out");

			screensManager.CloseWaitPanel(waitPanel);
			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@time_out") ,finishError);
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			resultText = "Unable to determine device location";
			Debug.Log("Unable to determine device location");

			screensManager.CloseWaitPanel(waitPanel);
			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@cannot_find_location"), finishError);
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			resultText = "Location: latitude " + Input.location.lastData.latitude + "\n longitude  " + Input.location.lastData.longitude + "\n altitude " + Input.location.lastData.altitude + "\n horizontalAccuracy " + Input.location.lastData.horizontalAccuracy + "\n lastData.timestamp " + Input.location.lastData.timestamp;

			longitude = Input.location.lastData.longitude;
			lattitude = Input.location.lastData.latitude;
		}

		Debug.Log (resultText);
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();

		screensManager.CloseWaitPanel(waitPanel);

		pSuccess ();
	}

	public void ShowStatus(){

			if (itIsGlobalStatus) {
				//turn on local status
				ShowLocalStatus ();
			} else {
				//turn on global status
				ShowGlobalStatus ();
			}

	}

	public void ShowRatingPanel(){
	
		StartCoroutine (ShowTop100(itIsGlobalStatus));
	}

	IEnumerator ShowTop100(bool pGlobalRat){

		string postScoreURL;
		string header;

		if (!pGlobalRat) {
			postScoreURL = NetWorkUtils.buildRequestGetLocalTopTen (longitude, lattitude);
			header = "Локальный";
		} else {
			postScoreURL = NetWorkUtils.buildRequestToGetTOP100 ();
			header = "Глобальный";
		}
		
//		var dictHeader = new Dictionary<string, string> ();
//		dictHeader.Add ("Content-Type", "text/json");
//
//		WWWForm form = new WWWForm ();
//		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL);

		while (!request.isDone) { 
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("Get friends URL ! " + request.url);
			Debug.Log ("Get friends done ! " + request.text);

			List<Friend> foundUsers;

			if (!pGlobalRat) {
				foundUsers = Utility.ParseFriendsJsonList (request.text, "GetLocalTopTenResult");
			} else {
				foundUsers = Utility.ParseFriendsJsonList (request.text, "GetTopTenResult");
			}

			postScoreURL = NetWorkUtils.buildRequestGetNumPlayers ();
			request = new WWW (postScoreURL);

			while (!request.isDone) { 
				yield return null;
			}

			var N = JSON.Parse(request.text);
			var mGetResult = N["GetNumPlayersResult"].AsInt;

			if (foundUsers != null && foundUsers.Count > 0) {				
				ratingDialog.ShowDialog(foundUsers, closeRatingDialog, header, mGetResult);
			} else {
				errorPanel = screensManager.ShowErrorDialog("Рейтинг не доступен.", finishError);
			}

		} else {

			Debug.LogError ("WWW Error: " + request.error);
			Debug.LogError ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика соединения с сервером", finishError);
		}

	}

	public void closeRatingDialog(){
	
	}

	IEnumerator RotateRings() {

		float startingTime = 0;

		while (gameObject.activeSelf) {

			startingTime += Time.deltaTime;

			GOgrayRing.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);
			GOcolorRing.transform.rotation = Quaternion.Euler (0, 0, -25 * startingTime);
			GOcaptionRing.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);

			if (startingTime > 360)
				startingTime = 0;

			yield return null;
		}
	}
}
