using Newtonsoft.Json;
using Supabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Database
{
    public class DBManager : MonoBehaviour
    {
        public static DBManager instance;
        
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Fetches all of the Leaderboard based on the given range
        /// </summary>
        /// <returns>Returns a list of all the players with respect to the Leaderboard Model</returns>
        //public async Task<List<Leaderboard>> GetLeaderboards(int max)
        //{
        //    try
        //    {
        //        var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Select(x => new object[] {x.LevelStats}).Limit(max).Get();
        //        return result.Models;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError($"Failed to retrieve top 10 leaderboard: {ex.Message}");
        //        return null;
        //    }
        //}

        public async Task<List<Leaderboard>> FetchLeaderboards(int sceneIndex, int max)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>().Where(x => x.sceneIndex == sceneIndex).Order(x => x.elapsedTime, Supabase.Postgrest.Constants.Ordering.Ascending).Limit(max).Get();

                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result.Models;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to fetch leaderboards: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddUserFromLeaderboard(string id, int sceneIndex, string playerName, float elapsedTime, int bookCollected)
        {
            try
            {
                // Retrieve the existing user from the leaderboard using the player_id
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>()
                    .Where(x => x.Player_ID == id)
                    .Get();

                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (result.Models.Count > 0)
                    {
                        var existingUser = result.Models.First();
                        existingUser.playerName = playerName;
                        existingUser.elapsedTime = elapsedTime;
                        existingUser.bookCollected = bookCollected;

                        var updateResult = await SupabaseClient.GetInstance().Client
                            .From<Leaderboard>().Where(x => x.Player_ID == id)
                            .Update(existingUser);

                        return updateResult.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK;
                    }
                    else
                    {
                        var newUser = new Leaderboard
                        {
                            Player_ID = id,
                            sceneIndex = sceneIndex,
                            playerName = playerName,
                            elapsedTime = elapsedTime,
                            bookCollected = bookCollected,
                            CreatedAt = DateTime.UtcNow 
                        };

                        var insertResult = await SupabaseClient.GetInstance().Client
                            .From<Leaderboard>()
                            .Insert(newUser);

                        return insertResult.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add or update leaderboard user: {ex.Message}");
                return false;
            }
        }

        public async Task<UserStatsModel> FetchData(string id)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<PlayerEntry>().Where(x => x.Player_ID == id).Get();

                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var usr = result.Model;

                    return new UserStatsModel
                    {
                        Name = usr.Name,
                        totalTime = usr.TotalTime,
                        totalBookmarks = usr.Bookmark,
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to fetch user {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdatePlayer(string id, UserStatsModel userStats)
        {
            try
            {
                Debug.Log($"User -> {id} Updating Data");

                var result = await SupabaseClient.GetInstance().Client
                    .From<PlayerEntry>()
                    .Where(x => x.Player_ID == id)
                    .Set(x => x.TotalTime, userStats.totalTime)
                    .Set(x => x.Bookmark, userStats.totalBookmarks)
                    .Update();

                return result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to update player on the leaderboards: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> insertPlayer(string username)
        {
            try
            {
                string player_id = System.Guid.NewGuid().ToString();

                var model = new PlayerEntry { Name = username, TotalTime = 0, Bookmark = 0, Player_ID = player_id};

                var result = await SupabaseClient.GetInstance().Client.From<PlayerEntry>().Insert(model);
                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    GameManager.GetInstance().SetUserID(player_id);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to insert user onto the leaderboards {ex.Message}");
                return false;
            }
        }
    }
}
