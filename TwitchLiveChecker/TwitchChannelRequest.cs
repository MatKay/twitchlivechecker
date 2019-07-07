using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLiveChecker
{
    class TwitchChannelRequest
    {
        private Newtonsoft.Json.Linq.JArray _data;
        private Newtonsoft.Json.Linq.JObject _pagination;

        public Newtonsoft.Json.Linq.JArray data { get => _data; set => _data = value; }
        public Newtonsoft.Json.Linq.JObject pagination { get => _pagination; set => _pagination = value; }
    }
}
