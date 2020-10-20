using Newtonsoft.Json;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace TwitchLiveChecker
{
    class TwitchChecker
    {
        public async Task<TwitchChannel> CheckChannel(string channel)
        {
            string response = await GetWebResponse(channel);

            TwitchChannelRequest test = JsonConvert.DeserializeObject<TwitchChannelRequest>(response);

            TwitchChannel channelObject = new TwitchChannel();

            if (test.data.HasValues)
            {
                foreach (var prop in test.data.First)
                {
                    JProperty jItem = (JProperty)prop;
                    var jItemName = jItem.Name;

                    switch (jItemName)
                    {
                        case "user_name":
                            channelObject.Name = jItem.Value.ToString();
                            break;

                        case "type":
                            channelObject.Status = jItem.Value.ToString();
                            break;

                        case "title":
                            channelObject.Title = jItem.Value.ToString();
                            break;

                        case "viewer_count":
                            channelObject.ViewerCount = jItem.Value.ToObject<int>();
                            break;

                        case "started_at":
                            channelObject.StartTime = DateTime.Parse(jItem.Value.ToString());
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                channelObject.Name = channel;
                channelObject.Status = "offline";
                channelObject.Title = null;
                channelObject.ViewerCount = null;
                channelObject.StartTime = null;
            }
            return await Task.FromResult<TwitchChannel>(channelObject);
        }

        private async Task<string> GetWebResponse(string channel)
        {
            string url = $"https://api.twitch.tv/helix/streams?user_login={channel}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Client-ID", _apikey);
            request.Accept = "application/vnd.twitchtv.v5+json";
            var response = await request.GetResponseAsync();

            var sr = new StreamReader(response.GetResponseStream());

            string webResponse = sr.ReadToEnd();

            return await Task.FromResult<String>(webResponse);
        }

    }
}
