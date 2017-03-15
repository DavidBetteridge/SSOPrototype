using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductA.Models
{
    public class SSO_Application
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ProductType { get; set; }

        public string URL { get; set; }

        public string ClientID { get; set; }

        public string ClientSecret { get; set; }

        public bool IsServer { get; set; }
    }
}