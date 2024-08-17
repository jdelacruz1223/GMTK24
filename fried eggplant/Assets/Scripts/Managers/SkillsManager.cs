using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{  
    public static SkillsManager instance;
    public LevelSkillStates skillStates;
    
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
    public void applySkill(Skill skill){
        //get data manager
        DataManager dataManager = DataManager.instance;

        if(skill.modifier.instantBoost != new Vector2(0,0)){
            dataManager.addBoost(skill.modifier.instantBoost);
        }
        if(skill.modifier.addBounce == true){
            dataManager.addBounce();
        }
        if(skill.modifier.addJump == true){
            dataManager.addJump();
        }
        print("Applied " + skill.name + " to player.");
    }
    public List<Skill> getAvailableSkills(){
        List<Skill> availableSkills = new List<Skill>();
        for(int i = 0; i < skillStates.allSkills.Length; i++){
            if(skillStates.isSkillAvailable[i]){
                availableSkills.Add(skillStates.allSkills[i]);
            }
        }
        return availableSkills;
    }
}