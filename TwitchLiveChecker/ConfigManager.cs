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
        private readonly string _filecontents;
        private readonly TwitchConfig _config;

        public ConfigManager()
        {
            _filecontents = File.ReadAllText(@"config.json");

            _config = JsonConvert.DeserializeObject<TwitchConfig>(_filecontents);
        }

        public string GetApiKey() => _config.ApiKey;
        public List<string> GetChannels() => _config.Channels;

        public void RemoveChannel(string ch)
        {
            _config.Channels.Remove(ch);
        }

        public void AddChannel(string name)
        {
            _config.Channels.Add(name);
        }

        public void SetApiKey(string key)
        {
            _config.ApiKey = key;
        }

        public void Save()
        {
            File.WriteAllText(@"config.json", JsonConvert.SerializeObject(_config));
        }
    }
}
