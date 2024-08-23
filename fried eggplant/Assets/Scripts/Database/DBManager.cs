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
        /// Fetch Leaderboard from Level Completed
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<List<Leaderboard>> FetchLeaderboards(int sceneIndex, int max)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>().Where(x => x.sceneIndex == sceneIndex).Order(x => x.elapsedTime, Supabase.Postgrest.Constants.Ordering.Ascending).Get();

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

        public async Task<int> GetScenes(string id)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>().Where(x => x.Player_ID == id).Get();

                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result.Models.Count;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to fetch leaderboards: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Leaderboards Menu
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<List<PlayerEntry>> FetchElapsedTimeLeaderboards()
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<PlayerEntry>().Order(x => x.TotalTime, Supabase.Postgrest.Constants.Ordering.Ascending).Get();

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

        /// <summary>
        /// Fetch Most Books Collected
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async Task<List<PlayerEntry>> FetchMostBookCollectedLeaderboards()
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<PlayerEntry>().Order(x => x.Bookmark, Supabase.Postgrest.Constants.Ordering.Descending).Get();

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
                        existingUser.elapsedTime = elapsedTime;
                        existingUser.bookCollected = bookCollected;

                        var updateResult = await SupabaseClient.GetInstance().Client
                            .From<Leaderboard>().Where(x => x.Player_ID == id)
                            .Update(existingUser);

                        Debug.Log(bookCollected);

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

                    Debug.Log(usr.Name);
                    Debug.Log(usr.TotalTime);
                    Debug.Log(usr.Bookmark);

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
