using System.Collections.Generic;

namespace TwitchLiveChecker
{
    class Config
    {
        
        public List<string> Channels { get; set; };
        public static Dictionary<string, string> oauth = new Dictionary<string, string>
        {
            { "authtoken", "" }

        };
    }
}
