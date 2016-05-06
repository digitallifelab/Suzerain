﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SimpleJSON;
using UnityEngine.Events;
using System.IO;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Facebook.Unity;


public class RegistrationScreenManager : MonoBehaviour {

	public InputField 	iUserName;
	public InputField 	iuserPassword;

	public Toggle		ExistUser;

	WaitPanel	waitPanel 	= null;
	ErrorPanel	errorPanel 	= null;
	
	float longitude = 0;
	float lattitude = 0;

	ScreensManager	screensManager	= null;

	public GameObject Google_Play;
	public GameObject FaceBook;
	public GameObject Login_Password;
	public GameObject TypeLogin_Panel;

	public InitSocialNetworks initNetwork;

	public void RegistrationType(int type){

		TypeLogin_Panel.SetActive (false);

		if (type == Constants.GOOGLE_PLAY) {
			Google_Play.SetActive (true);
			GoogleGetToken ();
		} else if (type == Constants.FACEBOOK) {
			FaceBook.SetActive (true);
			FBLogin ();
		} else if (type == Constants.LOGIN_PASS) {
			Login_Password.SetActive (true);
		}

	}

	void Start(){
		SoundManager.MusicOFF (true);
	}

	void OnEnable() {
	
		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Registration screen"));

		initNetwork.InitNetworks ();

		screensManager	= ScreensManager.instance;

		waitPanel = screensManager.ShowWaitDialog ("Определение местоположения ...");

		screensManager.InitLanguage ();
		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		Login_Password.SetActive (false);

		StartCoroutine( GetLocation () );

	}

//	public void OnLanguageChange(){
//
//		if (ScreensManager.LMan != null) {
//
//			if (english.isOn) {
//				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "English");
//				ScreensManager.LMan.setLanguageFromRes ("lang", "English");
//			} else {			
//				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "Russian");
//				ScreensManager.LMan.setLanguageFromRes ("lang", "Russian");
//			}
//
//			screensManager.TranslateUI ();
//		}
//	}

	public void finishError(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}
	
	public void finishRegistration()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext");

		AndroidJavaClass AdWordsConversionReporter = new AndroidJavaClass("com.google.ads.conversiontracking.AdWordsConversionReporter");
		AdWordsConversionReporter.CallStatic("reportWithConversionId", app, "925668342", "-0GECLO2-mQQ9qeyuQM", "0", true);
		#endif

		MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("game")
			.SetEventAction("Registration success"));
		
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
		SoundManager.MusicOFF (false);

		//Activate default view
		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

		screensManager.ShowMainScreen ();
	}
	
	public IEnumerator GetLocation()
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

			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@time_out") ,finishError);

			screensManager.CloseWaitPanel(waitPanel);

			yield break;
		}
		
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			resultText = "Unable to determine device location";
			Debug.Log("Unable to determine device location");


			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@cannot_find_location"), finishError);

			screensManager.CloseWaitPanel(waitPanel);

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
		
	}


	private void SaveUserPrefs(int pRegType)
	{
		UserController.UserName = iUserName.text;
		//UserController.eMail = ieMail.text;
		UserController.UserPassword = iuserPassword.text;

		PlayerPrefs.SetString ("username", 	UserController.UserName);
		PlayerPrefs.SetString ("email", 	UserController.eMail);
		PlayerPrefs.SetString ("password", 	UserController.UserPassword);
		PlayerPrefs.SetInt ("regType", 	pRegType);
	}
	
	public void onClickSubmitRegistration()
	{
		bool error = false;

		if (iUserName.text.Length == 0) {
			error = true;
			errorPanel = screensManager.ShowErrorDialog ("Имя пользователя должно быть заполнено.", finishError);
		}

//		if (!error && ieMail.text.Length == 0) {
//			error = true;
//			errorPanel = screensManager.ShowErrorDialog ("EMail должен быть заполнен.", finishError);
//		}

		if (!error && iuserPassword.text.Length < 5) {
			error = true;
			errorPanel = screensManager.ShowErrorDialog ("Длинна пароля должна быть не менее 5-ти символов.", finishError);
		}

		if (!error) {

			if (ExistUser.isOn ) {
				
				SaveExistUser ();

			} else {
					registerUser (iUserName.text
				, ""
				, iuserPassword.text
				, "0"//getSex ().ToString ()
				, getLanguageCode ().ToString ()
				, "102"
				, lattitude.ToString ()
				, longitude.ToString ()
					);
			}
		}

	}

	public void SaveExistUser(){
	
		UserController.UserName 		= iUserName.text;
		UserController.UserPassword 	= iuserPassword.text;
		//UserController.eMail 			= ieMail.text;
		PlayerPrefs.SetInt ("regType", 	3);

		UserController user_controller	= UserController.instance;

		waitPanel = screensManager.ShowWaitDialog(ScreensManager.LMan.getString ("@connecting"));
		user_controller.LogIn (InitLabels, longitude , lattitude);

	}

	public void InitLabels(bool error, string source_error, string error_text){

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (error == false) {
			
			SaveUserPrefs (1);
			UserController.registered 		= true;
			UserController.authenticated 	= false;
			screensManager.InitLanguage();

			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_success") , finishRegistration);
		} else {
			
			UserController.UserName 		= "";
			UserController.UserPassword 	= "";
			UserController.eMail 			= "";

			errorPanel = screensManager.ShowErrorDialog("Не верные учетные данные", finishError);
		}
	}

	public int getLanguageCode()
	{
//		if (english.isOn)
//			return 1;
		return 2;
	}
	
	private void registerUser(string pUserName, string pEmail, string pPassword, string pSex, string pLanguageId, string pCountry, string pLatitude, string pLongitude){
		
		Debug.Log ("Registration process");
		
		var postScoreURL = Utility.SERVICE_BASE_URL;		
		var method = Utility.REGISTER_USER_URL;
		
		var userName 		= "userName=";
		var eMail 			= "eMail=";
		var userPassword 	= "userPassword=";
		var sex 			= "sex=";
		var languageId 		= "languageId=";
		var countryId 		= "countryId=";
		var latitude 		= "latitude=";
		var longitude 		= "longitude=";
		var name 			= "name=";
		var id 				= "id=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ userName + pUserName + "&"
				+ eMail + "admin@gmail.com" + "&"
				+ sex + pSex + "&"
				+ userPassword + pPassword + "&"
				+ languageId + pLanguageId + "&"
				+ countryId + pCountry + "&"
				+ latitude + pLatitude + "&"
				+ longitude + pLongitude + "&"
				+ name + "hello" + "&"
				+ id + "333"+ "&"
				+ "ver=2"
				;
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);
		
		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add("Content-Type", "text/json");
		
		var request = new WWW(postScoreURL, null, dictHeader);

		waitPanel = screensManager.ShowWaitDialog (ScreensManager.LMan.getString("@registration"));

		StartCoroutine(WaitForRequest(request));
		
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			try{

				var N = JSON.Parse(www.text);
				var mLoginResult = N["RegisterUserResult"];

				User newUser = Utility.ParseGetUserResponse(mLoginResult.ToString());

				UserController.currentUser 	= newUser;
				UserController.registered 	= true;
				SaveUserPrefs(1);

				UserController.currentUser.Language = "Russian";

//				if (english.isOn) {
//					UserController.currentUser.Language = "English";
//				} else {
//					UserController.currentUser.Language = "Russian";
//				}


				screensManager.CloseWaitPanel(waitPanel);
				screensManager.InitLanguage();
				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_success") , finishRegistration);
			}
			catch(Exception e)
			{
				screensManager.CloseWaitPanel(waitPanel);
				errorPanel = screensManager.ShowErrorDialog(e.Message, finishError);
			}

		} else {

			screensManager.CloseWaitPanel(waitPanel);

			if(www.error.Contains("409") || www.error.Contains("conflict") || www.error.Contains("Conflict") || 
				www.text.Contains("409") || www.text.Contains("conflict") || www.text.Contains("Conflict")){
				//"Пользователь с таким именем " + iUserName.text + " существует, попробуйте другое имя."

				MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game")
					.SetEventAction("Registration error user exests"));
				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_user_exists"), finishError);
			}else{				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_error") , finishError);

				MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game")
					.SetEventAction("Registration error unknown"));
			}

			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	public void SucsessRegistration(int typeRed){
	
		string mStatusText = "Welcome " + Social.localUser.userName;

		mStatusText += "\n Email " + ((PlayGamesLocalUser)Social.localUser).Email;
		mStatusText += "\n Token ID is " + GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
		mStatusText += "\n AccessToken is " + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken();

		screensManager.CloseWaitPanel (waitPanel);
		errorPanel = screensManager.ShowErrorDialog(mStatusText ,finishError);
	}

	public void FailRegistration(int typeRed){

		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

		screensManager.CloseWaitPanel (waitPanel);
		errorPanel = screensManager.ShowErrorDialog("Authentication failed." ,finishError);

	}

	public void GoogleGetToken(){
		waitPanel = screensManager.ShowWaitDialog("Authenticating...");
		initNetwork.GoogleGetToken (SucsessRegistration, FailRegistration);
	}

	public void FBLogin(){
		
		waitPanel = screensManager.ShowWaitDialog("Authenticating...");
		initNetwork.FBLogin (SucsessRegistration, FailRegistration);
	}

}