﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CustomNumberTexFloat : MonoBehaviour {

	public Text num1; 
	public Text num2; 

	public Text num3; 
	public Text num4;

	Color32 defaultColor = new Color32(227,	189, 141, 255);

	public void SetNumber(float number){

		int sec1, sec2, milsec1, milsec2;
		float partAfterPoint;

		sec1 = (int)number / 10;
		sec2 = (int)number - sec1*10;
		partAfterPoint = (number - Mathf.Floor (number)) * 100;
		milsec1 = (int)partAfterPoint / 10;
		milsec2 = (int)partAfterPoint - milsec1 * 10;

		num1.text = sec1.ToString ();
		num2.text = sec2.ToString ();

		num3.text = milsec1.ToString ();
		num4.text = milsec2.ToString ();
	}

	public void SetRedColor(){

		num1.color = Color.red;
		num2.color = Color.red;

		num3.color = Color.red;
		num4.color = Color.red;
	}

	public void SetDefaultColor(){
		
		num1.color = defaultColor;
		num2.color = defaultColor;

		num3.color = defaultColor;
		num4.color = defaultColor;
	}
}
