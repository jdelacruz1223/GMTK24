using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Database
{
    public class UserDBManager : MonoBehaviour
    {
        public static UserDBManager instance;
        
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
        public async Task<List<Leaderboard>> GetLeaderboards(int max)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Order("total_time", Supabase.Postgrest.Constants.Ordering.Ascending).Limit(max).Get();
                return result.Models;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve top 10 leaderboard: {ex.Message}");
                return null;
            }
        }

        public async Task<UserStatsModel> FetchData(string id)
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>().Where(x => x.Player_ID == id).Get();


                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var usr = result.Model;

                    return new UserStatsModel
                    {
                        Name = usr.Name,
                        levelStats = usr.LevelStats,
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
                var result = await SupabaseClient.GetInstance().Client
                    .From<Leaderboard>()
                    .Where(x => x.Player_ID == id)
                    .Update(new Leaderboard { TotalTime = userStats.totalTime, Bookmark = userStats.totalBookmarks, LevelStats = userStats.levelStats });

                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
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

                var model = new Leaderboard { Name = username, TotalTime = 0, Bookmark = 0, LevelStats = new List<LevelModel>(), Player_ID = player_id};

                var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Insert(model);
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
