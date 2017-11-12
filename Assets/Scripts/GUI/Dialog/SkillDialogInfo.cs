using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDialogInfo : MonoBehaviour {
	public GameObject weightPriceLine;
	public GameObject price;
	public GameObject cdLine;
	public Text priceText;
	public GameObject weight;
	public Text weightText;
	public GameObject donatePrice;
	public Text donatePriceText; 
	public ScrollRectUpdater scrollRectUpdater;
	public DialogRectResizer dialogRectResizer;

	public Text title;
	public Text cd;
	public Text description;
	public Text commonDescriptionText;


	public Text skillEfficiency;


	public void SetInfo(int skillID, BackpackItem item = default(BackpackItem)){
		donatePrice.SetActive (false);
		if (item != null) {
			weightPriceLine.SetActive (true);
			dialogRectResizer.SetWeightPriceSize ();

			priceText.text = System.Math.Round (item.price, 2).ToString();
			weightText.text = System.Math.Round (item.weight, 2).ToString();

			if (item.donatePrice != 0) {
				donatePrice.SetActive (true);
				donatePriceText.text = System.Math.Round(item.donatePrice, 2).ToString();
			}


			commonDescriptionText.text = LanguageController.jsonFile ["skills"] ["backpackPageDescription"];
		} else {
			dialogRectResizer.SetFullSize ();
			weightPriceLine.SetActive (false);
			commonDescriptionText.text = LanguageController.jsonFile ["skills"] ["skillPageDescription"];
		}

		skillEfficiency.text = LanguageController.jsonFile ["skills"]["efficiency"] + ": " + PlayerController.skillStates[skillID].skillEfficiency.ToString() + "%";

		SkillSettings settings = SkillSettingsSet.GetSettings (skillID);

		title.text = settings.GetTitle ();
		if (settings.cooldown == 0) {
			cdLine.SetActive (false);
		} else {
			cdLine.SetActive (true);
			cd.text = LanguageController.jsonFile ["skills"] ["cd"] + ": " + Timer.GetTimeStringBySeconds (settings.GetCd());
		}
		description.text = settings.GetDescription ();

		scrollRectUpdater.UpdateRect ();

		DialogController.dialogController.currenBackpackItemInDialog = item;
	}
}
