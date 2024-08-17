using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Database
{
    public class LeaderboardManager : MonoBehaviour
    {
        /// <summary>
        /// Fetches all of the Leaderboard based on the given range
        /// </summary>
        /// <returns>Returns a list of all the players with respect to the Leaderboard Model</returns>
        public async Task<List<Leaderboard>> GetLeaderboards()
        {
            try
            {
                var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Get();
                return result.Models;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve top 10 leaderboard: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Inserts a player to the leaderboards
        /// </summary>
        /// <param name="player_name">Name of Player</param>
        /// <param name="score">BOOK_SCORE</param>
        /// <returns>A Boolean that will tell if player has been added to the leaderboards</returns>
        public async Task<bool> InsertPlayerToLeaderboards(string player_name, int score)
        {
            try
            {
                var model = new Leaderboard { Name = player_name, Score = score };

                var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Insert(model);
                if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve top 10 leaderboard: {ex.Message}");
                return false;
            }
        }
    }
}
