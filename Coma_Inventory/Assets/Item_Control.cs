using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item_Control : MonoBehaviour 
{
	public GameObject inventory;
	public GameObject anItem;
/*	public GameObject Deselected_Use;
	public GameObject Deselected_Examine;
	public GameObject Deselected_Quit;
	public GameObject Selected_Use;
	public GameObject Selected_Examine;
	public GameObject Selected_Quit;
	*/Item Item0;
	Item Item1;
	Item Item2;
	Item Item3;
	public GameObject I0;
	public GameObject I1;
	public GameObject I2;
	public GameObject I3;
	bool InventOpened;
	bool exit;
	bool inventory_focus;
	bool itemUse_focus;
	private int use_opt; //3 = use, 4 = examine, 5 = quit
	private int item_opt; //item_opt determines which item the three options is being selected through [indices 4-7]
	private int iter;

	public List<Item> ItemList;
	// Use this for initialization
	void Start () 
	{
		ItemList= new List<Item>();
		Item0 = new HealingItem();
		Item1 = new QuestItem1();
		Item2 = new QuestItem2();
		Item3 = new QuestItem3();
		ItemList.Add (Item0);
		ItemList.Add (Item1);
		ItemList.Add (Item2);
		ItemList.Add (Item3);
/*		
 		Deselected_Use = GameObject.Find ("Deselected_Use");
		Deselected_Examine = GameObject.Find ("Deselected_Examine");
		Deselected_Quit = GameObject.Find ("Deselected_Quit");
		Selected_Use = GameObject.Find ("Selected_Use");
		Selected_Examine = GameObject.Find ("Selected_Examine");
		Selected_Quit = GameObject.Find ("Selected_Quit");
		I0 = GameObject.Find ("Item0");
		I1 = GameObject.Find ("Item1");
		I2 = GameObject.Find ("Item2");
		I3 = GameObject.Find ("Item3");	
*/
		inventory = GameObject.Find ("Inventory_BG");
		for (iter = 0; iter < 12; iter++) 
		{
			if (iter < 6)
			{
				anItem = inventory.transform.GetChild (iter).gameObject;
				anItem.SetActive (false);
			}
			anItem = inventory.transform.GetChild(6).gameObject; //anItem becomes child at index(6)
			anItem.transform.GetChild (iter).gameObject.SetActive(false); //children of index(6) deactivated
		}
		inventory.SetActive (false);
		InventOpened = false;
		inventory_focus = true;
		itemUse_focus = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//This happens on every update while InventOpened is true
		if (InventOpened) 
		{
			inventory = this.transform.GetChild(0).gameObject;
			anItem = inventory.transform.GetChild(6).gameObject;

			//Initial:
			//setup occurs under Input.GetKeyUp(KeyCode.I)
			//mid:
				//Index [0] is the top, Index [3] is the bottom
			//anItem.transform.gameObject(x) is the text object
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				if (inventory_focus)
				{
					if (iter > 4) 
					{
						//if (ItemList[iter-1].obtained)
						anItem.transform.GetChild (iter).gameObject.SetActive (false); //deactivating current position;
						anItem.transform.GetChild (iter-1).gameObject.SetActive (true); //activate position higher on list
						iter--;
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
						anItem.transform.GetChild (iter).gameObject.SetActive (false); //deactivating current position;
						anItem.transform.GetChild (iter+1).gameObject.SetActive (true); //activating position lower on list
						iter++;
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



					if (use_opt == 3)
					{
						//bring up separate textbox asking if user would like to use item with yes/no options; control with arrow keys
						//if yes, implements the 'use' function of item 
						//if no, return to selecting use_opt (use/examine/quit)
					}

					if (use_opt == 4)
					{
						//display item description based on item_opt
					}

					if (use_opt == 5) 
					{
						//deactivate all 3 use/examine/quit options and return to iterating through item list
						for (int i = 3; i < 6; i++)	inventory.transform.GetChild (i).gameObject.SetActive (false);
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
					anItem = inventory.transform.GetChild(6).gameObject;
					anItem.SetActive(true);
					anItem.transform.GetChild (4).gameObject.SetActive (true);
					for (iter = 0; iter < ItemList.Count; iter++) //change to ItemList.Count to totalNumofItems (undetermined)
					{
						//if (ItemList[iter].obtained == true)
						anItem.transform.GetChild(iter).gameObject.SetActive(true);
					}
				}
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
						anItem = inventory.transform.GetChild (iter).gameObject;
						anItem.SetActive (false);
					}
					anItem = inventory.transform.GetChild(6).gameObject; //anItem becomes child at index(6)
					anItem.transform.GetChild (iter).gameObject.SetActive(false); //children of index(6) deactivated
				}
			}
		}
	}
}

public class Item
{
	protected string exam; //display when examined
	protected string description;
	protected string itemName;
	public bool obtained;
	public bool selected;
	protected int numItems;
	protected List<Text> TextList = new List<Text> ();
	Text itemText;	//image/sprite variable
	
	public Item()
	{
		this.exam = "This is a base Item.";
		this.itemName = "Base Item Name.";
		this.description = "The Parent of all Items.";
		this.obtained = false;
		this.numItems = 0;
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
		this.description = "It's a Cherry.";
		this.exam = "Heals 30% of your Health when used.";
		this.itemName = "Cherry.";
		this.obtained = false;
		this.numItems = 0;
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
		this.description = "This is Quest Item 1.";
		this.exam = "Examining Quest Item 1.";
		this.itemName = "Quest Item 1.";
		this.obtained = false;
		this.selected = false;
		this.numItems = 0;
	}
	public override void use(GameObject Player)
	{
		
	}
}
class QuestItem2 : Item
{
	public QuestItem2()
	{
		this.description = "This is Quest Item 2.";
		this.exam = "Examining Quest Item 2.";
		this.itemName = "Quest Item 2.";
		this.obtained = false;
		this.selected = false;
		this.numItems = 0;
	}
	public override void use(GameObject Player)
	{
		
	}
}
class QuestItem3 : Item
{
	public QuestItem3()
	{
		this.description = "This is Quest Item 3.";
		this.exam = "Examining Quest Item 3.";
		this.itemName = "Quest Item 3.";
		this.obtained = false;
		this.selected = false;
		this.numItems = 0;
	}
	public override void use(GameObject Player)
	{
		
	}
}
//Attach sprite to script object (Specific item classes controlling specific sprites/gameobjects)
//Enable/disable renderer on sprite for Inventory
//checks for collision with object in game universe using either collision with game player or click on box