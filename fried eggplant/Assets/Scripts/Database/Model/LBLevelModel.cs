using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;

[Table("levelstats")]
public class Leaderboard : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("sceneIndex")]
    public int sceneIndex { get; set; }

    [Column("player_id")]
    public string Player_ID { get; set; }

    [Column("player_name")]
    public string playerName { get; set; }

    [Column("elapsedTime")]
    public float elapsedTime { get; set; }

    [Column("bookCollected")]
    public int bookCollected { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}