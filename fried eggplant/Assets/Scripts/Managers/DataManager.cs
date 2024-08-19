using Assets.Scripts.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameObject player;
    public int extraJumpAmount = 0;
    public bool canBounce;
    public Vector2 instantBoostAmount = new Vector2(0, 0);
    public int booksAmount = 0;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject;
    }

    void Update()
    {

    }
    public Vector2 applyBoost()
    {
        Vector2 temp = instantBoostAmount;
        instantBoostAmount = new Vector2(0, 0);
        return temp;
    }
    public bool attemptToJumpAgain()
    {
        //returns if jump can be applied
        bool attempt = extraJumpAmount > 0;
        extraJumpAmount -= (extraJumpAmount > 0) ? 1 : 0;
        return attempt;
    }
    public void addJump()
    {
        extraJumpAmount += 1;
    }
    public void makeBouncey()
    {
        canBounce = true;
    }
    public void addBoost(Vector2 amt)
    {
        instantBoostAmount += amt;
    }
    public void addBook()
    {
        booksAmount += 1;
    }
    public void removeBook()
    {
        booksAmount -= 1;
    }
    public void removeBooks(int amt){
        for (int i = 0; i < amt; i++){
            StartCoroutine(player.GetComponent<BookCollector>().RemoveTopBook());
        }
    }
}