using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStatsModel
{
    public string Name { get; set; }
    public List<LevelModel> levelStats { get; set; }
    public float totalTime { get; set; }
    public int totalBookmarks { get; set; }
}
