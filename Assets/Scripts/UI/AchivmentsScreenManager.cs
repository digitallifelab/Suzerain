﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public delegate void GetLocationSuccessfully();

public class AchivmentsScreenManager : MonoBehaviour {

	public Text TextRating;
	public Text TextCount;
	public Text TextNickName;
	public Text ActiveStatus;
	public Text After;

//	public RectTransform grayRing;
//	public RectTransform colorRing;
//	public RectTransform captionRing;

	public GameObject GOgrayRing;
	public GameObject GOcolorRing;
	public GameObject GOcaptionRing;

	ScreensManager screensManager;

	ErrorPanel	errorPanel 	= null;
	WaitPanel	waitPanel 	= null;

	bool itIsGlobalStatus = true;

	float longitude = 0;
	float lattitude = 0;

	void OnEnable(){


		StartCoroutine(	RotateRings ());

		screensManager = ScreensManager.instance;

		InitAchivments ();

		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();
	}

	public void InitAchivments(){
	
		if (Rose.statList [0].Fights >= Constants.fightsCount) {
			TextRating.text = Rose.statList [0].GlobalStatus.ToString ();
			ActiveStatus.text = "Глобальный статус";
			After.text = "";
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

		if (Rose.statList [0].Fights >= Constants.fightsCount) {

			if (Rose.statList [0].Fights >= Constants.fightsCount) {
				After.text = "";
				TextRating.text = Rose.statList [0].LocalStatus.ToString ();

				if (lattitude == 0) {
					waitPanel = screensManager.ShowWaitDialog ("Определение местоположения");
					StartCoroutine (GetLocation (GetStatisticWithLocation));
				}

			} else {
				After.text = "Через";
				TextRating.text = (Constants.fightsCount - Rose.statList [0].Fights).ToString ();
			}
		}

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

	IEnumerator RotateRings() {

		//if (!Utility.StopCoroutine) {
			float startingTime = 0;

			while (gameObject.activeSelf) {

				startingTime += Time.deltaTime;

				GOgrayRing.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);
				GOcolorRing.transform.rotation = Quaternion.Euler (0, 0, -25 * startingTime);
				GOcaptionRing.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);

//				grayRing.rotation = Quaternion.Euler (0, 0, 25 * startingTime);
//				colorRing.rotation = Quaternion.Euler (0, 0, -25 * startingTime);
//				captionRing.rotation = Quaternion.Euler (0, 0, 25 * startingTime);

				//TextRating.transform.rotation = Quaternion.Euler (0, -25 * startingTime, 0);

				if (startingTime > 360)
					startingTime = 0;

				//yield return new WaitForSeconds(1);
				//Debug.Log (" Achivment Achivment Achivment Achivment Achivment !!!!!");
				yield return null;
			}
		//}
	}
}
