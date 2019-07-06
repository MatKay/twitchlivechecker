using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLiveChecker
{
    class TwitchChannel
    {
        private string _name;
        private string _status;

        public string Name { get => _name; set => _name = value; }
        public string Status { get => _status; set => _status = value; }

        public TwitchChannel(string name)
        {
            _name = name;
            _status = "unknown";
        }

        public TwitchChannel(string name, string status)
        {
            _name = name;
            _status = status;
        }
    }
}
