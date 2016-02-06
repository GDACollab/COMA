using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.UI;

using Object = System.Object;
using UnityObject = UnityEngine.Object;

/**
 * The player's inventory, containing all the items on their person.
 */
public class Inventory : MonoBehaviour
{
  private List<Item> items;
  
	void Start()
  {
    items = new List<Item>();
	}
  
  /**
   * Adds the given item to the inventory. The number of such items added will
   * be equal to the quantity field of the item instance provided.
   */
  public void Add(Item item)
  {
    Add(item, item.quantity);
  }
  
  /**
   * Adds the specified amount of the given item to the inventory.
   */
  public void Add(Item item, int amt)
  {
    int index = items.IndexOf(item);
    if(index >= 0)
      items[index].quantity += amt;
    else
      items.Add(item);
    items.Sort();
  }
  
  /**
   * Removes all of the given item from the inventory, then returns a reference
   * to the same; returns null if the inventory does not contain such an item.
   * If more items are removed than exist within, this method will only remove
   * and return as many as actually exist.
   */
  public Item Remove(Item item)
  {
    return Remove(item, item.quantity);
  }
  
  /**
   * Removes the given amount of the speified item from the inventory. Returns
   * the items removed, or null if they are not present. If more items are
   * removed than exist within, this method will only remove and return as many
   * as actually exist.
   */
  public Item Remove(Item item, int amt)
  {
    int index = items.IndexOf(item);
    if(index >= 0)
    {
      if(items[index].quantity > amt)
      {
        items[index].quantity -= amt;
        return item;
      }
      else
      {
        Item removed = items[index];
        items.RemoveAt(index);
        return removed;
      }
    }
    else
    {
      return null;
    }
  }
  
  /**
   * Returns the specified item from the inventory, if it exists; otherwise,
   * returns null. Beware that this will return a reference to the original
   * item, which opens the door to unscrupulous modification of its quantity.
   */
  public Item get(Item item)
  {
    int index = items.IndexOf(item);
    return (index >= 0) ? items[index] : null;
  }
  
  /**
   * Returns the item at the specified index. Beware that this will return a
   * reference to the original item, which opens the door to unscrupulous
   * modification of its quantity.
   */
  public Item get(int index)
  {
    return items[index];
  }
  
  /**
   * Returns whether the inventory contains the specified item.
   */
  public bool Contains(Item item)
  {
    return items.Contains(item);
  }
  
  /**
   * Clears all items from the inventory. Following this, the inventory will be
   * quite empty.
   */
  public void Clear()
  {
    items.Clear();
  }
  
  /**
   * Returns the number of items current held in the inventory.
   */
  public int Size()
  {
    return items.Count;
  }
  
  /**
   * Returns a read-only view of the inventory.
   */
  public ReadOnlyCollection<Item> AsReadOnly()
  {
    return items.AsReadOnly();
  }
  
  /**
   * Returns an array containing the elements of the inventory. Beware that the
   * array's elements will reference the original items.
   */
  public Item[] ToArray()
  {
    return items.ToArray();
  }
}

/**
 * An item held in the player's inventory. Each item has an name, examination
 * flavor text, and a quantity. Each instance of the type thus represents a
 * stack of equivalent items, such as three healing potions or a single vorpal
 * sword. Each item can also be operated, either conferring some effect on the
 * player or simply returning flavor text.
 */
[Serializable]
public abstract class Item : System.IComparable<Item>
{
  public string name;
  public string description;
  public string examineText;
  public int quantity;
  public bool obtained;
  public bool selected;
  
  /**
   * Creates a new item with the given name, examination flavor text, and
   * quantity. The flavor text is optional, but highly recommended.
   */
  public Item(string name, string description, string examineText = "An item.", int quantity = 1)
  {
    this.name = name;
    this.description = description;
    this.examineText = examineText;
    this.quantity = quantity;
    this.obtained = false;
    this.selected = false;
  }
  
  /**
   * Operate the item, conferring some effect.
   */
  public abstract void Operate(Object player);
  
  /**
   * Returns a hash code for this item.
   */
  public override int GetHashCode()
  {
    int hash = 1;
    hash += this.name.GetHashCode();
    hash *= 31;
    hash += this.examineText.GetHashCode();
    return hash;
  }
  
  /**
   * Returns whether this item and the other are equal. Items are equal if they
   * are of the same type and have the same name and examinaton flavor text.
   */
  public override bool Equals(Object o)
  {
    if(this == o)
      return true;
    else if(!(this.GetType() == o.GetType()))
      return false;
    else
    {
      Item item = (Item) o;
      return this.name == item.name
        && this. examineText == item.examineText;
    }
  }
  
  /**
   * Returns an integer indicating whther this item is less than, greater than,
   * or equal to the other.
   */
  public int CompareTo(Item item)
  {
    int comp = this.GetType().FullName.CompareTo(item.GetType().FullName);
    if(comp != 0)
      return comp;
    
    comp = this.name.CompareTo(item.name);
    if(comp != 0)
      return comp;
    
    return this.examineText.CompareTo(item.examineText);
  }
  
  /**
   * Returns a string representation of this item, of format [name] x[quanity].
   */
  public override string ToString()
  {
    return name + " x" + quantity;
  }
}

/**
 * A healing item, operable in or out of battle to heal the player.
 */
public class HealingItem : Item
{
  public int healingAmt;
  
  /**
   * Creates a new healing item of the given name and exmination flavor text,
   * quantity, and healing magnitude. The flavor text is optional, but highly
   * recommended.
   */
  public HealingItem(string name, string description, string examineText = "A healing item.",
    int healingAmt, int quantity = 1)
    : base(name, description, examineText, quantity)
  {
    this.healingAmt = healingAmt;
  }
  
  /**
   * Operates the healing item, restoring the player's health.
   */
  public override void Operate(Object player)
  {
    // Heal the player by healingAmt
  }
}

/**
 * A quest item, used as a condition for advancing quests.
 */
public class QuestItem : Item
{
  /**
   * Creates a new quest item with the given name, examination flavor text, and
   * quantity. The flavor text is optional, but highly recommended.
   */
  public QuestItem(string name, string description, string examineText = "A quest item.",
    int quantity = 1)
    :base(name, description, examineText, quantity) {}
  
  /**
   * Operates the quest item.
   */
  public override void Operate(Object player)
  {
    
  }
}
