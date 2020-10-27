using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TwitchConfig
{
    class Config
    {
        public List<string> Channels { get; set; }
        public Dictionary<string, string> oauth = new Dictionary<string, string>
        {
            { "authtoken", "" },
            { "refreshtoken", "" }
        };

        public static Config GetConfig()
        {
            if (File.Exists(SystemConfig.Environment["configfile"]))
            {
                string filecontents = File.ReadAllText(SystemConfig.Environment["configfile"]);
                Config config = JsonConvert.DeserializeObject<Config>(filecontents);

                return config;
            }
            else
            {
                Config config = new Config();
                config.Channels = new List<string> { "miekii", "darkviperau", "wardiii" };

                File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(config));
                return config;
            }
        }

        public bool Reload()
        {
            Config config;
            if (File.Exists(SystemConfig.Environment["configfile"]))
            {
                string filecontents = File.ReadAllText(SystemConfig.Environment["configfile"]);
                config = JsonConvert.DeserializeObject<Config>(filecontents);
            }
            else
            {
                config = new Config();
                config.Channels = new List<string> { "darkviperau", "miekii", "wardiii" };

                File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(config));
            }

            config.Channels.Sort();
            this.Channels = config.Channels;
            this.oauth = config.oauth;

            return true;
        }

        public bool Save()
        {
            this.Channels.Sort();
            File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(this));
            return true;
        }

        public static bool WriteConfig( Config config )
        {
            File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(config));
            return true;
        }

        public bool ChannelExists( string channel )
        {
            return this.Channels.Contains(channel.ToLower()) ? true : false;
        }

        public bool AddChannel( string channel )
        {
            if (ChannelExists(channel))
            {
                return false;
            }
            else
            {
                Channels.Add(channel.ToLower());
                Save();
                return true;
            }
        }

        public bool RemoveChannel( string channel )
        {
            if (ChannelExists(channel))
            {
                Channels.Remove(channel.ToLower());
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
