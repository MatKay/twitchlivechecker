using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchConfig
{
    class SystemConfig
    {
        public static Dictionary<string, string> Application = new Dictionary<string, string>
        {
            { "clientid", "drx70rx00ev71cq1jax7i4ji3ywib0" },
            { "clientsecret", "enrlbcqa312829q3e39q0z67kqgz9x" }
        };

        public static Dictionary<string, string> Environment = new Dictionary<string, string>
        {
            { "configfile", $"{Directory.GetCurrentDirectory()}/config.json" },
            { "redirect_uri", "http://localhost:58214" }
        };

        public static Dictionary<string, string> Twitch = new Dictionary<string, string>
        {
            { "oauth_loginurl",         "https://id.twitch.tv/oauth2/authorize" },
            { "oauth_tokenurl",         "https://id.twitch.tv/oauth2/token" },
            { "oauth_revocationurl",    "https://id.twitch.tv/oauth2/revoke" },
            { "api_channelurl",         "https://api.twitch.tv/helix/streams" },
            { "api_usersurl",            "https://api.twitch.tv/helix/users" }
        };
    }
}