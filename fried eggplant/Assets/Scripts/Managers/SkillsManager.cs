using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public Skill testSkill;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)){
            applySkill(testSkill);
        }
    }
}
