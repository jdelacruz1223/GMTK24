using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

[Table("leaderboard")]
public class Leaderboard : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("player_name")]
    public string Name { get; set; }

    [Column("score")]
    public int Score { get; set; }
}