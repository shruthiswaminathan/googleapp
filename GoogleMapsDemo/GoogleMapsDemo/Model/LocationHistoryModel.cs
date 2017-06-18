using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsDemo.Model
{
    public class LocationHistoryModel
    {
        public Location[] locations { get; set; }

        public class Location
        {
            public long timestampMs { get; set; }
            public int latitudeE7 { get; set; }
            public int longitudeE7 { get; set; }
            public int accuracy { get; set; }
            public int altitude { get; set; }
            public Activity[] activity { get; set; }
            public int heading { get; set; }
            public int velocity { get; set; }
        }

        public class Activity
        {
            public Activity1[] activity { get; set; }
            public string timestampMs { get; set; }
            
        }

        public class Activity1
        {
            public string type { get; set; }
            public int confidence { get; set; }
        }
    }
}
