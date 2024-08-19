using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{  
    public static SkillsManager instance;
    public LevelSkillStates skillStates;
    public Skill[] playerSkills; //all skills player can use
    public Skill playerCurrentSkill; //which skill is selected
    public int skillIndex = 0;
    
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
       playerSkills = getAvailableSkills().ToArray();
       playerCurrentSkill = playerSkills[skillIndex];
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(playerCurrentSkill != null){
                applySkill(playerCurrentSkill);
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0){
            if(playerCurrentSkill != null){ // bandaid, but if index 0 has no skills, neither will 1-n
                GetPreviousSkill();
                print("Switched to skill " + playerCurrentSkill.name);
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0){
            if(playerCurrentSkill != null){ // bandaid, but if index 0 has no skills, neither will 1-n
                GetNextSkill();
                print("Switched to skill " + playerCurrentSkill.name);
            }
        }
    }
    void GetPreviousSkill(){
        skillIndex -= 1;
        if(skillIndex < 0){
            skillIndex = playerSkills.Length-1;
        }
        playerCurrentSkill = playerSkills[skillIndex];
    }
    void GetNextSkill(){
        skillIndex += 1;
        if(skillIndex >= playerSkills.Length){
            skillIndex = 0;
        }
        playerCurrentSkill = playerSkills[skillIndex];
    }
    public void applySkill(Skill skill){
        //get data manager
        if(DataManager.instance.booksAmount < skill.cost){
            return;
        }
        if(!DataManager.instance.player.GetComponent<PlayerMovement>().isControllable()){
            return;
        }
        else{
            DataManager.instance.removeBooks(skill.cost);
            SkillsUIManager.GetInstance().updateVisualUsed();
        }

        if (skill.modifier.instantBoost != new Vector2(0,0)){
            DataManager.instance.addBoost(skill.modifier.instantBoost);
        }
        if(skill.modifier.addBounce == true){
            DataManager.instance.makeBouncey();
        }
        if(skill.modifier.addJump == true){
            DataManager.instance.addJump();
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