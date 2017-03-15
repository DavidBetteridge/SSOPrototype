using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductA.ViewModels
{
    public class SSO_TokenResponse
    {
        //{"access_token":"ACCESS_TOKEN","token_type":"bearer","expires_in":2592000,"refresh_token":"REFRESH_TOKEN","scope":"read","uid":100101,"info":{"name":"Mark E. Mark","email":"mark@thefunkybunch.com"}}
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_id { get; set; }  //Not used
        public string refresh_token { get; set; }  //Not used
        public string scope { get; set; }
        public string uid { get; set; }
        public string info { get; set; }

    }
}