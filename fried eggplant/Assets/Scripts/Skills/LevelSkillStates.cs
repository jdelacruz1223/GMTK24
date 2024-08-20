using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Books/Level Skill States/Create New Level Skill States", fileName = "NewSkillStates")]
public class LevelSkillStates : ScriptableObject
{
    public Skill[] allSkills;
    public bool[] isSkillAvailable;
    public bool loaded = false;

    void OnEnable(){
        if(!loaded){
            allSkills = Resources.LoadAll<Skill>("Skills");
            isSkillAvailable = new bool[allSkills.Length];
            loaded = true;
        }
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(LevelSkillStates))]
public class LevelSkillStatesEditor : Editor{

    private SerializedProperty allSkills;
    private SerializedProperty isSkillAvailable;

    void OnEnable(){
        allSkills = serializedObject.FindProperty("allSkills");
        isSkillAvailable = serializedObject.FindProperty("isSkillAvailable");
    }

    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();

        if(allSkills.arraySize == 0 && isSkillAvailable.arraySize == 0){
            EditorGUILayout.HelpBox("Play a scene once and this object should become populated with all the available skills.", MessageType.Info);
        }
        else{
            serializedObject.Update();
            EditorGUILayout.LabelField("Which skills are available in the level?");
            for(int i = 0; i < allSkills.arraySize; i++){
                SerializedProperty skillName = allSkills.GetArrayElementAtIndex(i);
                SerializedProperty skillBool = isSkillAvailable.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(skillName.objectReferenceValue.name);
                bool newSkillBool = EditorGUILayout.Toggle(skillBool.boolValue);
                if(newSkillBool != skillBool.boolValue){
                    skillBool.boolValue = newSkillBool;
                }
                EditorGUILayout.EndHorizontal();
            }
            serializedObject.ApplyModifiedProperties();
        }        
    }
}
#endif