using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item_Control : MonoBehaviour 
{
	public GameObject inventory;
	public GameObject theCanvas;
	Item Item0;
	Item Item1;
	Item Item2;
	Item Item3;
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
	Text Item0_Selected_Text;
	Text Item1_Selected_Text;
	Text Item2_Selected_Text;
	Text Item3_Selected_Text;
	Text Item0_Description_Text;
	Text Item1_Description_Text;
	Text Item2_Description_Text;
	Text Item3_Description_Text;
	public List<Item> ItemList;
	// Use this for initialization
	void Start () 
	{
		//ItemList is actually completely useless, but its count variable is used in calculating certain values in Update. x_x meh.
		ItemList = new List<Item> ();
		Item0 = new HealingItem ();
		Item1 = new QuestItem1 ();
		Item2 = new QuestItem2 ();
		Item3 = new QuestItem3 ();
		ItemList.Add (Item0);
		ItemList.Add (Item1);
		ItemList.Add (Item2);
		ItemList.Add (Item3);

		inventory = GameObject.Find ("Inventory_BG");
		for (iter = 0; iter < 12; iter++) {
			if (iter < 6) {
				theCanvas = inventory.transform.GetChild (iter).gameObject; //temporarily use theCanvas to refer to all use/examine/quit selections
				theCanvas.SetActive (false); //deactivate all use/examine/quit selections
			}
			theCanvas = inventory.transform.GetChild (6).gameObject; //theCanvas becomes child at index(6)
			theCanvas.transform.GetChild (iter).gameObject.SetActive (false); //children of index(6) deactivated
		}
		//Making text objects manipulable in script. 
		//example usage: Item0_Deselected_Text.text = Item0_Selected_Text.text = "Item_Name0"
		Item0_Deselected_Text = theCanvas.transform.GetChild (0).GetComponent<Text>();
		Item1_Deselected_Text = theCanvas.transform.GetChild (1).GetComponent<Text>();
		Item2_Deselected_Text = theCanvas.transform.GetChild (2).GetComponent<Text>();
		Item3_Deselected_Text = theCanvas.transform.GetChild (3).GetComponent<Text>();
		Item0_Selected_Text = theCanvas.transform.GetChild (4).GetComponent<Text>();
		Item1_Selected_Text = theCanvas.transform.GetChild (5).GetComponent<Text>();
		Item2_Selected_Text = theCanvas.transform.GetChild (6).GetComponent<Text>();
		Item3_Selected_Text = theCanvas.transform.GetChild (7).GetComponent<Text>();
		Item0_Description_Text = theCanvas.transform.GetChild (8).GetComponent<Text>();
		Item1_Description_Text = theCanvas.transform.GetChild (9).GetComponent<Text>();
		Item2_Description_Text = theCanvas.transform.GetChild (10).GetComponent<Text>();
		Item3_Description_Text = theCanvas.transform.GetChild (11).GetComponent<Text>();
		inventory.SetActive (false);
		InventOpened = false;
		inventory_focus = true;
		itemUse_focus = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//****DEFINING COLLISION AND COLLECTION OF ITEMS****//
		//Somehow item is either collided against or collected.

		//This happens on every update while InventOpened is true
		if (InventOpened) 
		{
			inventory = this.transform.GetChild(0).gameObject;
			theCanvas = inventory.transform.GetChild(6).gameObject;

			//Initial:
			//setup occurs under Input.GetKeyUp(KeyCode.I)
			//mid:
				//Index [0] is the top, Index [3] is the bottom
			//theCanvas.transform.gameObject(x) is the text object
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				if (inventory_focus)
				{
					if (iter > 4) 
					{
						//if (ItemList[iter-1].obtained)
						theCanvas.transform.GetChild (iter).gameObject.SetActive (false); //deactivating current position;
						theCanvas.transform.GetChild (iter-1).gameObject.SetActive (true); //activate position higher on list
						theCanvas.transform.GetChild (iter+4).gameObject.SetActive (false);
						iter--;
						theCanvas.transform.GetChild (iter+4).gameObject.SetActive (true);
					}
				}
				if (itemUse_focus) 
				{
					if (use_opt == 5) 
					{
						//if (ItemList[iter-1].obtained)
						inventory.transform.GetChild (use_opt).gameObject.SetActive (false); //deactivating current position;
						use_opt = 3;
						inventory.transform.GetChild (use_opt).gameObject.SetActive (true); //activate position higher on list
					}
				}
			}
			if (Input.GetKeyUp(KeyCode.DownArrow)){
				if (inventory_focus)
				{
					if (iter < ItemList.Count+3) 
					{
						//if (ItemList[iter+1].obtained)
						theCanvas.transform.GetChild (iter).gameObject.SetActive (false); //deactivating current position;
						theCanvas.transform.GetChild (iter+1).gameObject.SetActive (true); //activating position lower on list
						theCanvas.transform.GetChild (iter+4).gameObject.SetActive (false);
						iter++;
						theCanvas.transform.GetChild (iter+4).gameObject.SetActive (true);
					}
				}
				if (itemUse_focus)
				{
					if (use_opt == 3 || use_opt == 4) 
					{
						//if (ItemList[iter-1].obtained)
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
						//if (ItemList[iter-1].obtained)
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
						if (item_opt == 4) Item0_Description_Text.text = Item0.exam;
						if (item_opt == 5) Item1_Description_Text.text = Item1.exam;
						if (item_opt == 6) Item2_Description_Text.text = Item2.exam;
						if (item_opt == 7) Item3_Description_Text.text = Item3.exam;
						theCanvas.transform.GetChild (item_opt+4).gameObject.SetActive (true); //activating examination text

					}

					if (use_opt == 5) 
					{
						//deactivate all 3 use/examine/quit options and return to iterating through item list
						for (int i = 3; i < 6; i++)	inventory.transform.GetChild (i).gameObject.SetActive (false);
//						theCanvas.transform.GetChild (item_opt+4).gameObject.SetActive (false); //deactivating description text if any
						Item0_Description_Text.text = Item0.description;
						Item1_Description_Text.text = Item1.description;
						Item2_Description_Text.text = Item2.description;
						Item3_Description_Text.text = Item3.description;
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

		//This happens only when 'I' is pressed:
		if (Input.GetKeyUp (KeyCode.I)) 
		{
			Item0_Description_Text.text = Item0.description;
			Item1_Description_Text.text = Item1.description;
			Item2_Description_Text.text = Item2.description;
			Item3_Description_Text.text = Item3.description;
			theCanvas.transform.GetChild (8).gameObject.SetActive (true); //activating examination text
			inventory = this.transform.GetChild(0).gameObject;
			inventory.SetActive (!InventOpened);
			//Initial-default display:
			//display inventory background
			//display USE and EXAMINE sprites (deselected)
			//display all item sprites (deselected)
			InventOpened = !InventOpened;
			if (InventOpened)
			{
				for (iter = 0; iter < 3; iter++) 
				{
					inventory.transform.GetChild (iter).gameObject.SetActive (true);
				}
				if (ItemList.Count != 0) {
					ItemList[0].selected = true;
					theCanvas = inventory.transform.GetChild(6).gameObject;
					theCanvas.SetActive(true);
					theCanvas.transform.GetChild (4).gameObject.SetActive (true); //have first item already selected
					for (iter = 0; iter < ItemList.Count; iter++) //change to ItemList.Count to totalNumofItems (undetermined)
					{
						//if (ItemList[iter].obtained == true)
						theCanvas.transform.GetChild(iter).gameObject.SetActive(true);
					}
				}
				Item0_Selected_Text.text = Item0_Deselected_Text.text = Item0.itemName;
				Item1_Selected_Text.text = Item1_Deselected_Text.text = Item1.itemName;
				Item2_Selected_Text.text = Item2_Deselected_Text.text = Item2.itemName;
				Item3_Selected_Text.text = Item3_Deselected_Text.text = Item3.itemName;
				/*Item0_Description_Text.text = "You're gonna have a \nbad time.";
				Item1_Description_Text.text = "LET ME MAKE YOU \nSPAGHETTI";
				Item2_Description_Text.text = "OMG! MewMew \nKissy! \nSo! \nKyute!! \nOMGG!! X3";
				Item3_Description_Text.text = "Kill or be killed. \nThat's the way it is \ndown here.";*/
				Item0_Description_Text.text = Item0.description;
				Item1_Description_Text.text = Item1.description;
				Item2_Description_Text.text = Item2.description;
				Item3_Description_Text.text = Item3.description;
				iter = 4; //set to position of objs
				inventory_focus = true;
				itemUse_focus = false;
				use_opt = 3;
			}
			//when inventory is closed:
			if (!InventOpened) {
				//Deactivate everything
				for (iter = 0; iter < 12; iter++) {
					if (iter < 6){
						theCanvas = inventory.transform.GetChild (iter).gameObject;
						theCanvas.SetActive (false);
					}
					theCanvas = inventory.transform.GetChild(6).gameObject; //theCanvas becomes child at index(6)
					theCanvas.transform.GetChild (iter).gameObject.SetActive(false); //children of index(6) deactivated
				}
			}
		}
	}
}

public class Item
{
	public string exam; //display when examined
	public string description;
	public string itemName;
	public bool obtained;
	public bool selected;
	protected int numItems;
	
	public Item()
	{
		this.numItems = 0;
		this.exam = "This is a base Item.";
		this.itemName = "Base Item Name.";
		this.description = "The Parent of all Items.";
		this.obtained = false;
		this.selected = false;
	}
	public void examine()
	{
	
	}
	public void pickup() //checks for collision with object in game universe using either collision with game player or click in boxcollision
	{
		
	}
	public virtual void display (int x, int offset){

	}
	public virtual void use(GameObject Player){}
}

class HealingItem : Item
{
	public HealingItem()
	{
		this.numItems = 0;
		this.description = "Healing Item \nDescription.";
		this.exam = "Heals 30% of your \nHealth when used.";
		this.itemName = "Item0 x" + this.numItems;
		this.obtained = false;
		this.selected = false;
	}
	public override void use(GameObject Player)
	{
		//Get HealthPoint Script or gameObject
		int Health = 0;// = Player.GetComponent<HP> ();
		if (Health < 100) {
			if (this.numItems > 0) {
				Health += 30;
				this.numItems--;
				if (Health > 100) Health = 100;
				if (numItems == 0) this.obtained = false;
			}
		}
	}
	
	public override void display (int x, int offset)
	{
		if (this.obtained) {
			//display item in list in specified positions
		
			//new Vector3 ((float)x, (offset*(-.35f)), 0);
			//if selected, do further display of item description
			if (selected) {

			}
		}
		
	}
}
class QuestItem1 : Item
{	
	public QuestItem1()
	{
		this.numItems = 0;
		this.description = "This is Quest \nItem 1.";
		this.exam = "Examining Quest \nItem 1.";
		this.itemName = "Item1 x" + this.numItems;
		this.obtained = false;
		this.selected = false;
	}
	public override void use(GameObject Player)
	{
		
	}
}
class QuestItem2 : Item
{
	public QuestItem2()
	{
		this.numItems = 0;
		this.description = "This is Quest \nItem 2.";
		this.exam = "Examining Quest \nItem 2.";
		this.itemName = "Item2 x" + this.numItems;
		this.obtained = false;
		this.selected = false;
	}
	public override void use(GameObject Player)
	{
		
	}
}
class QuestItem3 : Item
{
	public QuestItem3()
	{
		this.numItems = 0;
		this.description = "This is Quest \nItem 3.";
		this.exam = "Examining Quest \nItem 3.";
		this.itemName = "Item3 x" + this.numItems;
		this.obtained = false;
		this.selected = false;;
	}
	public override void use(GameObject Player)
	{
		
	}
}
//Attach sprite to script object (Specific item classes controlling specific sprites/gameobjects)
//Enable/disable renderer on sprite for Inventory
//checks for collision with object in game universe using either collision with game player or click on box