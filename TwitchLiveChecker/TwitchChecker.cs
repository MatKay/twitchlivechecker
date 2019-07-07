using Newtonsoft.Json;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TwitchLiveChecker
{
    class TwitchChecker
    {
        private string _apikey;

        public TwitchChecker(string key)
        {
            _apikey = key;
        }

        public TwitchChannel CheckChannel(string channel)
        {
            var response = GetWebResponse(channel);

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
            return channelObject;
        }

        private string GetWebResponse(string channel)
        {
            string url = $"https://api.twitch.tv/helix/streams?user_login={channel}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Client-ID", _apikey);
            var response = request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

    }
}
