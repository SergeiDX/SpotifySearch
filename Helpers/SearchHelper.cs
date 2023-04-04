using Newtonsoft.Json;
using RestSharp;
using SpotifySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SpotifySearch.Models.SpotifySearch;

namespace SpotifySearch.Helpers
{
    public static class SearchHelper
    {
        public static Token token { get; set; }

        public static async Task GetTokenAsync()
        {
            #region SecretVault
            string clientID = "41bb3e95853047699c0bad73a41fe977";

            string clientSecret = "c6a26ff8c34c4125bb0cf9290440edd4";
            #endregion


            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientID + ":" +clientSecret));

            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("grant_type", "client_credentials")
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
            HttpContent content = new FormUrlEncodedContent(args);

            HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            string msg = await resp.Content.ReadAsStringAsync();

            token = JsonConvert.DeserializeObject<Token>(msg);
        }

        public static SpotifyResult SearchArtistOrSong(string searchWord)
        {
            var client = new RestClient("https://api.spotify.com/v1/search");
            client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
            var request = new RestRequest($"?q={searchWord}&type=artist", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
