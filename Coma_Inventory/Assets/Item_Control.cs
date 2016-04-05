using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Object = System.Object;
using UnityObject = UnityEngine.Object;

public class Item_Control : MonoBehaviour 
{
	public GameObject inventory;
	public GameObject theCanvas;
	Item Item0;
	Item Item1;
	Item Item2;
	Item Item3;
	Item Item4;
	Item Item5;
	Item Item6;
	Item Item7;
	Item Item8;
	Item Item9;
	Item Item10;
	Item Item11;

	bool InventOpened;
	bool exit;
	bool inventory_focus;
	bool itemUse_focus;
	private int use_opt; //3 = use, 4 = examine, 5 = quit
	private int item_opt; //item_opt determines which item the three options is being selected through [indices 4-7]
	private int iter;
	Text Item0_Deselected_Text;
	Text Item1_Deselected_Text;
	Text Item2_Deselected_Text;
	Text Item3_Deselected_Text;
	Text Item4_Deselected_Text;
	Text Item5_Deselected_Text;
	Text Item6_Deselected_Text;
	Text Item7_Deselected_Text;
	Text Item8_Deselected_Text;
	Text Item9_Deselected_Text;
	Text Item10_Deselected_Text;
	Text Item11_Deselected_Text;
	Text Item0_Selected_Text;
	Text Item1_Selected_Text;
	Text Item2_Selected_Text;
	Text Item3_Selected_Text;
	Text Item4_Selected_Text;
	Text Item5_Selected_Text;
	Text Item6_Selected_Text;
	Text Item7_Selected_Text;
	Text Item8_Selected_Text;
	Text Item9_Selected_Text;
	Text Item10_Selected_Text;
	Text Item11_Selected_Text;
	Text Description_Text; //default description text

	public List<int> ItemOptList;
	Inventory itemss;
	// Use this for initialization
	void Start () 
	{
		itemss = new Inventory ();
		Item0 = new HealingItem ("Cherry.", "This is a\nCherry.", 30, "Heals 30% of \nyour Health.", 1);
		Item1 = new QuestItem ("Sword.", "This is a\nSword.", "It has a cute\ntattoo on its\nhilt.", 2); 
		Item2 = new QuestItem ("Boob.", "It's a boob.", "Still a boob.", 1);
		itemss.Add (Item0);
		itemss.Add (Item1);
		itemss.Add (Item2);

		//(string name, string description, string examineText = "A quest item.",
		//int quantity = 1)

		//(string name, string description, string examineText = "A healing item.",
		//int healingAmt, int quantity = 1)
		inventory = GameObject.Find ("Inventory_BG");
		theCanvas = inventory.transform.GetChild (6).gameObject; //theCanvas is the Canvas at index(6), child of Inventory_BG; do not change this variable
		for (iter = 0; iter < 25; iter++) {
			if (iter < 6) {
				inventory.transform.GetChild (iter).gameObject.SetActive(false); //Deactivating everything under Inventory_BG
			}

			theCanvas.transform.GetChild (iter).gameObject.SetActive (false); //children of index(6) deactivated
		}
		//Making text objects manipulable in script. 
		//example usage: Item0_Deselected_Text.text = Item0_Selected_Text.text = "Item_Name0"
		Item0_Deselected_Text = theCanvas.transform.GetChild (0).GetComponent<Text>();
		Item1_Deselected_Text = theCanvas.transform.GetChild (1).GetComponent<Text>();
		Item2_Deselected_Text = theCanvas.transform.GetChild (2).GetComponent<Text>();
		Item3_Deselected_Text = theCanvas.transform.GetChild (3).GetComponent<Text>();
		Item4_Deselected_Text = theCanvas.transform.GetChild (4).GetComponent<Text>();
		Item5_Deselected_Text = theCanvas.transform.GetChild (5).GetComponent<Text>();
		Item6_Deselected_Text = theCanvas.transform.GetChild (6).GetComponent<Text>();
		Item7_Deselected_Text = theCanvas.transform.GetChild (7).GetComponent<Text>();
		Item8_Deselected_Text = theCanvas.transform.GetChild (8).GetComponent<Text>();
		Item9_Deselected_Text = theCanvas.transform.GetChild (9).GetComponent<Text>();
		Item10_Deselected_Text = theCanvas.transform.GetChild (10).GetComponent<Text>();
		Item11_Deselected_Text = theCanvas.transform.GetChild (11).GetComponent<Text>();
		Item0_Selected_Text = theCanvas.transform.GetChild (12).GetComponent<Text>();
		Item1_Selected_Text = theCanvas.transform.GetChild (13).GetComponent<Text>();
		Item2_Selected_Text = theCanvas.transform.GetChild (14).GetComponent<Text>();
		Item3_Selected_Text = theCanvas.transform.GetChild (15).GetComponent<Text>();
		Item4_Selected_Text = theCanvas.transform.GetChild (16).GetComponent<Text>();
		Item5_Selected_Text = theCanvas.transform.GetChild (17).GetComponent<Text>();
		Item6_Selected_Text = theCanvas.transform.GetChild (18).GetComponent<Text>();
		Item7_Selected_Text = theCanvas.transform.GetChild (19).GetComponent<Text>();
		Item8_Selected_Text = theCanvas.transform.GetChild (20).GetComponent<Text>();
		Item9_Selected_Text = theCanvas.transform.GetChild (21).GetComponent<Text>();
		Item10_Selected_Text = theCanvas.transform.GetChild (22).GetComponent<Text>();
		Item11_Selected_Text = theCanvas.transform.GetChild (23).GetComponent<Text>();
		Description_Text = theCanvas.transform.GetChild (24).GetComponent<Text>();

		inventory.SetActive (false);
		InventOpened = false;
		inventory_focus = true;
		itemUse_focus = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//****DEFINING COLLISION AND COLLECTION OF items****//
		//Somehow item is either collided against or collected.

		//***********TO OPEN INVENTORY PRESS 'I'*************//
		if (Input.GetKeyUp (KeyCode.I)) 
		{
			theCanvas.transform.GetChild (24).gameObject.SetActive (true); //activating Description text
			inventory = this.transform.GetChild(0).gameObject;
			inventory.SetActive (!InventOpened);
			//Initial-default display:
			//display inventory background
			//display USE and EXAMINE sprites (deselected)
			//display all item sprites (deselected)
			InventOpened = !InventOpened;
			if (InventOpened)
			{
				//ACTIVATE THE DESELETED USE/EXAMINE/QUIT options:
				for (iter = 0; iter < 3; iter++) 
				{
					inventory.transform.GetChild (iter).gameObject.SetActive (true);
				}

				//Do operations based on Count of items on items:
				if (itemss.Size() != 0)
				{
					Description_Text.text = itemss.get (0).description; //Default Description_Text is description of first item
					theCanvas = inventory.transform.GetChild(6).gameObject;
					theCanvas.SetActive(true);
					theCanvas.transform.GetChild (12).gameObject.SetActive (true); //have first item already selected
					for (iter = 0; iter < itemss.Size (); iter++)
					{
						theCanvas.transform.GetChild(iter).gameObject.SetActive(true); //SetActive all the deselected item
						if (iter == 0) {
							Item0_Selected_Text.text = Item0_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 1) {
							Item1_Selected_Text.text = Item1_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 2) {
							Item2_Selected_Text.text = Item2_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 3) {
							Item3_Selected_Text.text = Item3_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 4) {
							Item4_Selected_Text.text = Item4_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 5) {
							Item5_Selected_Text.text = Item5_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 6) {
							Item6_Selected_Text.text = Item6_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 7) {
							Item7_Selected_Text.text = Item7_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 8) {
							Item8_Selected_Text.text = Item8_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 9) {
							Item9_Selected_Text.text = Item9_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 10) {
							Item10_Selected_Text.text = Item10_Deselected_Text.text = itemss.get(iter).name;
						}
						if (iter == 11) {
							Item11_Selected_Text.text = Item11_Deselected_Text.text = itemss.get(iter).name;
						}
					}
					iter = 0; //set iter equal to the index of the first item
					inventory_focus = true;
					itemUse_focus = false;
					use_opt = 3;
				} else {
					//else, itemss.Size() is 0, there are no items in the inventory:
					iter = -1;
					Description_Text.text = "";
				}
				//OPENING inventory displays only items that exist in the item list, and the above makes sure of that
			}
			//when inventory is closed:
			if (!InventOpened) {
				//Deactivate everything
				for (iter = 0; iter < 25; iter++) {
					if (iter < 6){
						inventory.transform.GetChild (iter).gameObject.SetActive(false); //deactivating all objects under Inventory_BG
					}
					theCanvas.transform.GetChild (iter).gameObject.SetActive(false);//Deactivating children of index(6) which are all the items on the lefthand list
				}
			}
		}
//This happens on every update while InventOpened is true
		if (InventOpened) 
		{
			inventory = this.transform.GetChild(0).gameObject;
			theCanvas = inventory.transform.GetChild(6).gameObject; //possibly redundant

			//*******ARROW KEYS CHANGE THE VARIABLE ITER, WHICH DETERMINES WHICH ITEM IS BEING SELECTED*********//
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				if (inventory_focus)
				{
					if (iter > 0) 
					{
						theCanvas.transform.GetChild (iter+12).gameObject.SetActive (false); //deactivating current position;
						theCanvas.transform.GetChild (iter+11).gameObject.SetActive (true); //activate position higher on list
						iter--;
						Description_Text.text = itemss.get(iter).description;
					}
				}
				if (itemUse_focus) 
				{
					if (use_opt == 5) 
					{
						inventory.transform.GetChild (use_opt).gameObject.SetActive (false); //deactivating current position;
						use_opt = 3;
						inventory.transform.GetChild (use_opt).gameObject.SetActive (true); //activate position higher on list
					}
				}
			}
			if (Input.GetKeyUp(KeyCode.DownArrow)){
				if (inventory_focus)
				{
					if (iter < itemss.Size () -1 && iter != -1) 
					{
						theCanvas.transform.GetChild (iter+12).gameObject.SetActive (false); //deactivating current position;
						theCanvas.transform.GetChild (iter+13).gameObject.SetActive (true); //activating position lower on list
						iter++;
						Description_Text.text = itemss.get(iter).description;
					}
				}
				if (itemUse_focus)
				{
					if (use_opt == 3 || use_opt == 4) 
					{
						inventory.transform.GetChild (use_opt).gameObject.SetActive (false); //deactivating current position;
						use_opt = 5;
						inventory.transform.GetChild (use_opt).gameObject.SetActive (true); //activate position higher on list
					}
				}
			}
			if (Input.GetKeyUp(KeyCode.RightArrow)){
				if (itemUse_focus)
				{
					if (use_opt == 5 || use_opt == 3) 
					{
						inventory.transform.GetChild (use_opt).gameObject.SetActive (false); //deactivating current position;
						use_opt = 4;
						inventory.transform.GetChild (use_opt).gameObject.SetActive (true); //activate position higher on list
					}
				}
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow)){
				if (itemUse_focus)
				{
					if (use_opt == 4) 
					{
						inventory.transform.GetChild (use_opt).gameObject.SetActive (false); //deactivating current position;
						use_opt = 3;
						inventory.transform.GetChild (use_opt).gameObject.SetActive (true); //activate position higher on list
					}
				}
			}
			if (Input.GetKeyUp(KeyCode.Return))
			{
				if (itemUse_focus) 
				{
					//PUT CODE HERE FOR USE, EXAMINE AND QUIT BASED ON THE ITEM BEING SELECTED (check item_opt 4-7 for items 0-3 respectively)
					//Activate use, examine, or quit based on use_opt (3 = use, 4 = examine, 5 = quit)
					//***FIRST SET DESCRIPTION TEXT***//
					if (use_opt == 3)
					{
						//bring up separate textbox asking if user would like to use item with yes/no options; control with arrow keys
						//if yes, implements the 'use' function of item 
						//if no, return to selecting use_opt (use/examine/quit)
					}

					if (use_opt == 4)
					{
						//Change Description Text to Examine Text depending on item_opt
						theCanvas.transform.GetChild (24).gameObject.SetActive (true); //activating description text
						if (item_opt == 0) Description_Text.text = itemss.get (0).examineText;
						if (item_opt == 1) Description_Text.text = itemss.get (1).examineText;
						if (item_opt == 2) Description_Text.text = itemss.get (2).examineText;
						if (item_opt == 3) Description_Text.text = itemss.get (4).examineText;
						if (item_opt == 4) Description_Text.text = itemss.get (5).examineText;
						if (item_opt == 5) Description_Text.text = itemss.get (6).examineText;
						if (item_opt == 6) Description_Text.text = itemss.get (7).examineText;
						if (item_opt == 7) Description_Text.text = itemss.get (8).examineText;
						if (item_opt == 8) Description_Text.text = itemss.get (9).examineText;
						if (item_opt == 9) Description_Text.text = itemss.get (10).examineText;
						if (item_opt == 10) Description_Text.text = itemss.get (11).examineText;
						if (item_opt == 11) Description_Text.text = itemss.get (12).examineText;
					}

					if (use_opt == 5) 
					{
						//deactivate all 3 SELECTED use/examine/quit options and return to iterating through item list
						for (int i = 3; i < 6; i++)	inventory.transform.GetChild (i).gameObject.SetActive (false);
//						theCanvas.transform.GetChild (item_opt+4).gameObject.SetActive (false); //deactivating description text if any
						use_opt = 3;
						itemUse_focus = false;
						inventory_focus = true;
						//deactivate any item description texts that are active
					}
				} else if (inventory_focus) {
					inventory_focus = false;
					itemUse_focus = true;
					item_opt = iter;
					inventory.transform.GetChild (use_opt).gameObject.SetActive (true);
				}
			}
		}
			
			
			//If item is selected, turn on selected for clicked item and turn off for others (or use arrows)
			//If USE or EXAMINE is selected, check which item is selected if any change USE/EXAMINE sprite. THEN USE/EXAMINE the item
			//If another item is selected, revert USE and EXAMINE sprites back to normal
			//Item have base descriptions separate from EXAMINE
	}
}

