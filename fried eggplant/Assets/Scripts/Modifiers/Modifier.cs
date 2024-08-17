using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Books/Modifiers/Create New Modifier", fileName = "New Modifier")]
public class Modifier : ScriptableObject
{
    public Vector2 instantBoost; //apply instantBoost to velocity of player. override maxSpeed
    public bool addBounce; //add a bounce to a count, while count is not 0, bounce off a surface
    public bool addJump; //add jump to a count, while count is not 0 allow more jumps
}
