using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitchLiveChecker
{
    // https://www.newtonsoft.com/json/help/html/SerializingJSON.htm

    // https://api.twitch.tv/helix/streams?user_login=wubbl0rz
    // Client-ID: bbloyjh0wzg9fdeplksluv1gs27865

    class ConfigManager
    {
        //private readonly string _filecontents;
        // readonly TwitchConfig _config;

        //public ConfigManager()
        //{
            //_filecontents = File.ReadAllText(@"config.json");

            //_config = JsonConvert.DeserializeObject<TwitchConfig>(_filecontents);
        //}

        //public string GetApiKey() => _config.ApiKey;
        //public List<string> GetChannels() => _config.Channels;

        public void RemoveChannel(string ch)
        {
            string channel_delete = null;

            foreach (string channel in _config.Channels)
            {
                if (channel.ToLower() == ch.ToLower())
                {
                    channel_delete = channel;
                    break;
                }
            }

            if (channel_delete == null)
            {
                throw new System.Exception();
            }
            else
            {
                _config.Channels.Remove(channel_delete);
            }
        }

        public void AddChannel(string name)
        {
            _config.Channels.Add(name);
        }

        public void SetApiKey(string key)
        {
            _config.ApiKey = key;
        }

        private void SortChannels()
        {
            //_config.Channels;
             _config.Channels = _config.Channels.OrderBy(a => a).ToList();
        }

        public void Save()
        {
            SortChannels();
            File.WriteAllText(@"config.json", JsonConvert.SerializeObject(_config));
        }
    }
}
