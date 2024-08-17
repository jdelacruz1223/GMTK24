using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Books/Skills/Create New Skill", fileName = "New Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public string description;
    public int cost;
    public Modifier modifier;
}
