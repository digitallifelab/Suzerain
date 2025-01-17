﻿using UnityEngine;
using System.Collections;

public class TestTask
{
	public int FightId { get; set; }
	public int QNum { get; set; }
	public int TaskId { get; set; }
	public string TextQuestion { get; set; }
	public string Ans1 { get; set; }
	public string Ans2 { get; set; }
	public string Ans3 { get; set; }
	public string Ans4 { get; set; }
	public byte[] PicQuestion { get; set; }
	public byte[] Var1 { get; set; }
	public byte[] Var2 { get; set; }
	public byte[] Var3 { get; set; }
	public byte[] Var4 { get; set; }
	public int TrueValue { get; set; }
	public int WasBought { get; set; }

	public string GetRightAnswer(){
		if (TrueValue == 1) {
			return Ans1;
		} else if (TrueValue == 2) {
			return Ans2;
		} else if (TrueValue == 3) {
			return Ans3;
		} else if (TrueValue == 4) {
			return Ans4;
		} else {
			return "";
		}
	}
}
