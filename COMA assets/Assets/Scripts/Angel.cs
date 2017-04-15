using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Comments from James:
//   I've removed all of "MarshalledState" from the original code. 
//   I'm not quite sure why the original writer did that.

public class Angel : MonoBehaviour
{
    public static Vector2 transitionPos; // New position after a field transition
    public static string currScene; // Keep track of scene for returning from battles

    // Other global state
    public static float hp;
    public static Inventory inventory = new Inventory();
    public static Item Item0 = new HealingItem("Cherry.", "This is a Cherry.", 30, "Heals 30% of \nyour Health.", 1);
    public static Item Item1 = new QuestItem("Sword.", "This is a Sword.", "It has a cute \ntattoo on its hilt.", 2);
    public static Item Item2 = new QuestItem("Boob.", "It's a boob.", "Still a boob.", 1);

    public void Start()
    {
        hp = 100f;
    }

    public static void TransitionFromBattleToField()
    {
        Debug.Log(transitionPos);
        Debug.Log(currScene);
        SceneManager.LoadScene(currScene);
    }

    public static void TransitionFromFieldToBattle(string destination)
    {
        transitionPos = GameObject.FindGameObjectWithTag("Player").transform.position; // Keep track of old position
        currScene = SceneManager.GetActiveScene().name; // Keep track of old scene
        Debug.Log(transitionPos);
        Debug.Log(currScene);
        SceneManager.LoadScene(destination);
    }

    // Load new field scene and set new position for Blue
    public static void TransitionFromFieldToField(string destination)
    {
        SceneManager.LoadScene(destination); // Load new scene
        ChangeArea ca = GameObject.FindGameObjectWithTag("Background").GetComponent<ChangeArea>();
        if (ca.LeftLevelLoad == destination)
        { // Going left
            transitionPos = new Vector2(1.7f, -0.65f);
        }
        else if (ca.RightLevelLoad == destination)
        { // Going right
            transitionPos = new Vector2(-1.6f, -0.65f);
        }
    }
}