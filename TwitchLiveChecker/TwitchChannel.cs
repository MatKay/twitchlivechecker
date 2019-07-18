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
        private string _title;
        private int? _viewercount;
        private DateTime? _starttime;
        private string _thumbnail;

        public string Name { get => _name; set => _name = value; }
        public string Status { get => _status; set => _status = value; }
        public string Title { get => _title; set => _title = value; }
        public int? ViewerCount { get => _viewercount; set => _viewercount = value; }
        public DateTime? StartTime { get => _starttime; set => _starttime = value; }
        public string Thumbnail { get => _thumbnail; set => _thumbnail = value; }

        public TwitchChannel(string name)
        {
            _name = name;
            _status = "unknown";
            _title = null;
        }

        public TwitchChannel(string name, string status)
        {
            _name = name;
            _status = status;
            _title = null;
        }

        public TwitchChannel()
        {

        }

    }
}
