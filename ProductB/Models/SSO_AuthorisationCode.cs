using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductB.Models
{
    public class SSO_AuthorisationCode
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public DateTime Timestamp { get; set; }

        public int SSO_ApplicationID { get; set; }

        public string State { get; set; }

        public string SourceUserID { get; set; }

        public string LocalUserID { get; set; }

        public bool ExistingLink { get; set; }

    }
}