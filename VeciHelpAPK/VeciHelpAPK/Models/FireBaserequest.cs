using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
    public class Rootobject
    {
        public string[] registration_ids { get; set; }
        public string collapse_key { get; set; }
        public Notification notification { get; set; }
        public Data data { get; set; }
    }

    public class Notification
    {
        public string body { get; set; }
        public string title { get; set; }
    }

    public class Data
    {
        public string body { get; set; }
        public string title { get; set; }
        public string key_1 { get; set; }
        public string key_2 { get; set; }
    }


}
