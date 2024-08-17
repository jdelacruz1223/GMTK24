using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSkillView : MonoBehaviour
{   
    Text skillList;

    // Start is called before the first frame update
    void Start()
    {
       skillList = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        skillList.text = updateList();
    }
    string updateList(){
        string list = "";
        for(int i = 0; i < SkillsManager.instance.playerSkills.Length; i++){
            if(SkillsManager.instance.playerSkills[i] == SkillsManager.instance.playerCurrentSkill){
                list += "#" + SkillsManager.instance.playerSkills[i].name + "#";
            }
            else{
                list += SkillsManager.instance.playerSkills[i].name;
            }
            if(i != SkillsManager.instance.playerSkills.Length-1){
                list += '\n';
            }
        }
        foreach(Skill s in SkillsManager.instance.playerSkills){
            
        }
        return list;
    }
}
