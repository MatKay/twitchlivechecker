using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace TwitchLiveChecker
{
    class ConfigManager
    {
        private string _filecontents;
        private TwitchConfig _config;

        public ConfigManager()
        {
            _filecontents = File.ReadAllText(@"config.json");

            _config = JsonConvert.DeserializeObject<TwitchConfig>(_filecontents);
        }

        public string GetApiKey() => _config.ApiKey;
        public string[] GetChannels() => _config.Channels;

        public void RemoveChannel(string ch)
        {
            _config.Channels = _config.Channels.Where(val => val != ch).ToArray();
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

    class TwitchConfig
    {
        private string apikey;
        private string[] channels;

        public string ApiKey { get => apikey; set => apikey = value; }
        public string[] Channels { get => channels; set => channels = value; }

        public TwitchConfig(string apikey, string[] channels)
        {
            this.apikey = apikey;
            this.channels = channels;
        }
    }
}
