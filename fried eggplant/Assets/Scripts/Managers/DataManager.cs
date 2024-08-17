using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public int playerAddtlJumpAmount = 0;
    public int playerBounceAmount = 0;
    public Vector2 playerInstantBoostAmount = new Vector2(0,0);


    private void Awake(){
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
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2 applyBoost(){ 
        Vector2 temp = playerInstantBoostAmount;
        playerInstantBoostAmount = new Vector2(0,0);
        return temp;
    }
    public bool attemptToJumpAgain(){
        //returns if jump can be applied
        bool attempt = playerAddtlJumpAmount>0;
        playerAddtlJumpAmount -= (playerAddtlJumpAmount>0)?1:0;
        return attempt;
    }
    public bool attemptToBounce(){
        bool attempt = playerBounceAmount>0;
        playerBounceAmount -= (playerBounceAmount>0)?1:0;
        return attempt;
    }
    public void addJump(){
        playerAddtlJumpAmount += 1;
    }
    public void addBounce(){
        playerBounceAmount += 1;
    }
    public void addBoost(Vector2 amt){
        playerInstantBoostAmount += amt;
    }
}
