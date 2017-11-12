using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsView : MonoBehaviour {
	public static BuffsView buffsView;
	public RectTransform statsRect;
	public List<Buff> currentBuffs = new List<Buff>();
	public List<BuffButton> buttons = new List<BuffButton> ();

	public GameObject line1;
	public GameObject line2;
	public GameObject line3;


	public RectTransform line0ActiveRect;
	public RectTransform line1ActiveRect;
	public RectTransform line2ActiveRect;
	public RectTransform line3ActiveRect;


	void Awake(){
		buffsView = this;
	}


	public static void AddBuff(Buff buff){
		buffsView.currentBuffs.Add (buff);
		RedrawBuffs ();
	}

	public static void RemoveBuff (Buff buff){
		int removableBuffID = buffsView.currentBuffs.FindIndex (b => b.buffID == buff.buffID);
		if (removableBuffID != -1) {
			buffsView.currentBuffs.RemoveAt (removableBuffID);
		}
		RedrawBuffs ();
	}

	public static void ChangeBuffTime(float currentTime, float maximumBuffTime, int buffID){
		int index = buffsView.buttons.FindIndex (b => b.buffID == buffID);
		buffsView.buttons [index].SetTime (currentTime, maximumBuffTime);
	}

	public static void RedrawBuffs(){
		foreach (BuffButton button in buffsView.buttons) {
			button.button.SetActive (false);
		}
		if (buffsView.currentBuffs.Count == 0) {
			Set0LineActive ();
		} else if (buffsView.currentBuffs.Count > 0 && buffsView.currentBuffs.Count <= 4) {
			Set1LineActive ();
		} else if (buffsView.currentBuffs.Count > 4 && buffsView.currentBuffs.Count <= 8) {
			Set2LineActive ();
		} else if (buffsView.currentBuffs.Count > 8 && buffsView.currentBuffs.Count <= 12) {
			Set3LineActive ();
		}

		for (int i = 0; i < buffsView.currentBuffs.Count; i++) {
			buffsView.buttons [i].SetButton (buffsView.currentBuffs [i]);
		}
	}


	public static void Set0LineActive(){
		buffsView.line1.SetActive (false);
		buffsView.line2.SetActive (false);
		buffsView.line3.SetActive (false);
		buffsView.statsRect.sizeDelta = buffsView.line0ActiveRect.sizeDelta;
		buffsView.statsRect.localPosition = buffsView.line0ActiveRect.localPosition;
	}

	public static void Set1LineActive(){
		buffsView.line1.SetActive (true);
		buffsView.line2.SetActive (false);
		buffsView.line3.SetActive (false);
		buffsView.statsRect.sizeDelta = buffsView.line1ActiveRect.sizeDelta;
		buffsView.statsRect.localPosition = buffsView.line1ActiveRect.localPosition;
	}

	public static void Set2LineActive(){
		buffsView.line1.SetActive (true);
		buffsView.line2.SetActive (true);
		buffsView.line3.SetActive (false);
		buffsView.statsRect.sizeDelta = buffsView.line2ActiveRect.sizeDelta;
		buffsView.statsRect.localPosition = buffsView.line2ActiveRect.localPosition;
	}

	public static void Set3LineActive(){
		buffsView.line1.SetActive (true);
		buffsView.line2.SetActive (true);
		buffsView.line3.SetActive (true);
		buffsView.statsRect.sizeDelta = buffsView.line3ActiveRect.sizeDelta;
		buffsView.statsRect.localPosition = buffsView.line3ActiveRect.localPosition;
	}

}
