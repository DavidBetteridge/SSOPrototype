using System.Collections.Generic;

namespace ProductB.Models
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

        public virtual ICollection<SSO_ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<SSO_AuthorisationCode> AuthorisationCodes { get; set; }
    }
}