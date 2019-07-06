using System.Collections.Generic;

namespace TwitchLiveChecker
{
    class TwitchConfig
    {
        private string apikey;
        private List<string> channels;

        public string ApiKey { get => apikey; set => apikey = value; }
        public List<string> Channels { get => channels; set => channels = value; }

        public TwitchConfig(string apikey, List<string> channelarray)
        {
            this.apikey = apikey;
            if (!(channelarray == null))
            {
                foreach (string channel in channelarray)
                {
                    channels.Add(channel);
                }
            }
            
        }
    }
}
