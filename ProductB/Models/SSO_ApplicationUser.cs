namespace ProductB.Models
{
    public class SSO_ApplicationUser
    {
        public int ID { get; set; }

        public int SSO_ApplicationID { get; set; }

        public string SourceUserID { get; set; }

        public string LocalUserID { get; set; }
    }
}