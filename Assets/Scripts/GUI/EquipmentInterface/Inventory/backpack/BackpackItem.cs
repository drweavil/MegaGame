using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class BackpackItem{
	public int itemID;
	public List<object> itemContent = new List<object>();
	public int itemCount = 1;
	public float weight=0;
	public float price=0;
	public float donatePrice = 0;
	public const int maximumItems = 100;


	public List<BackpackItem> CombineItems(BackpackItem item){
		List<BackpackItem> items = new List<BackpackItem> ();

		if (this.itemCount + item.itemCount > maximumItems) {
			int allItemsCount = this.itemCount + item.itemCount;
			itemCount = maximumItems;
			allItemsCount -= maximumItems;
			//float currentItemCount = 0;
			MethodInfo method = item.itemContent [0].GetType ().GetMethod ("Clone");
			object newItemContent = method.Invoke (item.itemContent [0], null);
			BackpackItem newItem = new BackpackItem ();

			newItem.price = (float)System.Math.Round(GetUnitPrice () * allItemsCount, 2);
			newItem.weight = (float)System.Math.Round(GetUnitWeight () * allItemsCount, 2);
			newItem.itemCount = allItemsCount;
			newItem.itemContent.Add (newItemContent);

			items.Add (newItem);


		} else {
			float currentUnitPrice = GetUnitPrice ();
			float currentUnitWeight = GetUnitWeight ();

			itemCount = this.itemCount + item.itemCount;
			weight = itemCount * currentUnitWeight;
			price = itemCount * currentUnitPrice;
		}

		return items;
	}

	public void ChangeItemCount(int newCount){
		float currentUnitWeight = GetUnitWeight ();
		float currentUnitPrice = GetUnitPrice ();

		itemCount = newCount;
		weight = (float)System.Math.Round(itemCount * currentUnitWeight, 2);
		price = (float)System.Math.Round(itemCount * currentUnitPrice, 2);
	}

	public float GetUnitPrice(){
		return (float)System.Math.Round(price / itemCount, 2);
	}

	public float GetUnitWeight(){
		return (float)System.Math.Round(weight / itemCount, 2);
	}
}
