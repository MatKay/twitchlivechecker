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
                string _filecontents = File.ReadAllText(SystemConfig.Environment["configfile"]);
                Config _config = JsonConvert.DeserializeObject<Config>(_filecontents);

                return _config;
            }
            else
            {
                Config _config = new Config();
                _config.Channels = new List<string> { "Miekii", "DarkViperAU", "Wardiii" };

                File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(_config));
                return _config;
            }

            
        }

        public bool Save()
        {
            File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(this));
            return true;
        }

        public static bool WriteConfig( Config _config )
        {
            File.WriteAllText(SystemConfig.Environment["configfile"], JsonConvert.SerializeObject(_config));
            return true;
        }
    }
}
