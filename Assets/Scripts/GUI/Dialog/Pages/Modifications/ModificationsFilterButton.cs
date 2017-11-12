using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificationsFilterButton : MonoBehaviour {
	public int statID;
	public GameObject activated;



	public void Activate(){
		foreach (ModificationsFilterButton button in ModificationsPage.modificationsPage.filterButtons) {
			button.activated.SetActive (false);
		}
		activated.SetActive (true);

		ModificationsPage.modificationsPage.SortRunesByStatID (statID);
		ModificationsPage.modificationsPage.currenRuneItemsPage = 1;
		ModificationsPage.modificationsPage.DrawRunes ();

	}
}
