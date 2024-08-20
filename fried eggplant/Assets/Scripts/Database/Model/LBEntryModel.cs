using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

[Table("leaderboard")]
public class Leaderboard : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Column("player_id")]
    public string Player_ID { get; set; }

    [Column("player_name")]
    public string Name { get; set; }

    [Column("total_bookmarks")]
    public int Bookmark { get; set; }

    [Column("levelstats")]
    public List<LevelModel> LevelStats { get; set; }

    [Column("totaltime")]
    public float TotalTime { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}