using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TwitchConfig;

namespace TwitchLiveChecker
{
    class TwitchAPI
    {
        public async Task<List<TwitchChannel>> GetChannelsAsync(string[] channels)
        {
            if (channels.Length >= 100)
            {
                throw new NotImplementedException("Querying more than 100 streams at a time is currently not supported!");
            }

            Config config = Config.GetConfig();

            RestClient httpclient = new RestClient(SystemConfig.Twitch["api_channelurl"]);
            RestRequest request = new RestRequest(Method.GET);
            RestRequest retryrequest = new RestRequest(Method.GET);
            request.AddHeader("client-id", SystemConfig.Application["clientid"]);
            request.AddHeader("Authorization", $"Bearer {config.oauth["authtoken"]}");

            foreach (string channel in channels)
            {
                request.AddParameter("user_login", channel);
                retryrequest.AddParameter("user_login", channel);
            }
            

            IRestResponse httpresponse = await httpclient.ExecuteAsync(request);

            if (httpresponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                config = TwitchOAuthHandler.RefreshOAuthToken();
                retryrequest.AddHeader("client-id", SystemConfig.Application["clientid"]);
                retryrequest.AddHeader("Authorization", $"Bearer {config.oauth["authtoken"]}");

                httpresponse = await httpclient.ExecuteAsync(retryrequest);
            }
            
            if (httpresponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TwitchAPIChannelResponse apiresponse = JsonConvert.DeserializeObject<TwitchAPIChannelResponse>(httpresponse.Content);

                TwitchChannel[] channelarray = apiresponse.data;

                return new List<TwitchChannel>(channelarray);
            }
            else
            {
                return new List<TwitchChannel>();
            }
        }

        public async Task<string> GetLoggedinUserAsync()
        {
            Config config = Config.GetConfig();
            RestClient httpclient = new RestClient(SystemConfig.Twitch["api_userurl"]);
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("Client-ID", SystemConfig.Application["clientid"]);
            request.AddHeader("Authorization", $"Bearer {config.oauth["authtoken"]}");

            IRestResponse response = await httpclient.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TwitchUserResponse responseobj = JsonConvert.DeserializeObject<TwitchUserResponse>(response.Content);
                return responseobj.data.First().display_name;
            }
            else
            {
                RestRequest retryrequest = new RestRequest(Method.GET);
                retryrequest.AddHeader("Client-ID", SystemConfig.Application["clientid"]);

                config = TwitchOAuthHandler.RefreshOAuthToken();
                retryrequest.AddHeader("Authorization", $"Bearer {config.oauth["authtoken"]}");
                IRestResponse retryresponse = await httpclient.ExecuteAsync(retryrequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TwitchUserResponse responseobj = JsonConvert.DeserializeObject<TwitchUserResponse>(response.Content);
                    return responseobj.data.First().display_name;
                }
                else
                {
                    return "";
                }
            }

        }
    }
}
