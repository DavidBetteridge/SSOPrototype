using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductA.ViewModels
{
    public class SSO_TokenExchange
    {
        internal string grant_type;

        public string state { get; set; }

        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string scope { get; set; }

        public string code { get; set; }
    }
}