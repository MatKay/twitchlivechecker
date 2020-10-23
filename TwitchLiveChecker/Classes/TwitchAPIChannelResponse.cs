using Newtonsoft.Json.Linq;

namespace TwitchLiveChecker
{
    class TwitchAPIChannelResponse
    {
        public TwitchChannel[] data { get ; set; }
        public JObject pagination { get; set; }
    }
}
