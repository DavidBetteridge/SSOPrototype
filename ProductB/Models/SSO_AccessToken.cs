using System;

namespace ProductB.Models
{
    public class SSO_AccessToken
    {
        public int ID { get; set; }

        public string Token { get; set; }

        public DateTime Timestamp { get; set; }
        public string LocalUserID { get; set; }
        public int SSO_ApplicationID { get; set; }
    }
}