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
        public async Task GetLeaderboards()
        {
            try
            {
                var model = new Leaderboard
                {
                    Name = "Dichill",
                    Score = 0,
                };

                var response = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Insert(model);
                Debug.Log(response.ResponseMessage);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to retrieve top 10 leaderboard: {ex.Message}");
            }
        }


    }
}
