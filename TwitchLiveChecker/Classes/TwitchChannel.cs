using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLiveChecker
{
    class TwitchChannel
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string game_id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int viewer_count { get; set; }
        public string started_at { get; set; }
        public string language { get; set; }
        public string thumbnail_url { get; set; }
        public string[] tag_ids { get; set; }

        public DateTime StartTime
        {
            get
            {
                return DateTime.Parse(started_at);
            }
        }

        public string LiveString
        {
            get
            {
                if (type == "live")
                {
                    TimeSpan ts = DateTime.Now.Subtract(StartTime);
                    return $"live for {ts.Hours}h {ts.Minutes}m";
                }
                else
                {
                    return "offline";
                }
            }
        }

        public TwitchChannel(string username)
        {
            user_name = username;
            type = "unknown";
            title = "";
        }

        public TwitchChannel(string username, string status)
        {
            user_name = username;
            type = status;
            type = "";
        }

        public TwitchChannel()
        {

        }
    }
}
