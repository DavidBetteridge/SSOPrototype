using ProductA.ViewModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductA.Controllers
{
    public class SSOController : Controller
    {
        // GET: SSO
        public ActionResult Index(int productID)
        {
            using (var ctx = new ProductDB())
            {
                var app = ctx.SSO_Applications.SingleOrDefault(a => a.ID == productID);
                if (app == null) throw new System.Exception("There is no product with an id of " + productID);
                return View(app);
            }

        }


        public ActionResult CallBack(string state, string authorisationCode)
        {
            var viewModel = new SSO_Response()
            {
                AuthorisationCode = authorisationCode,
                State = state
            };

            return View(viewModel);
        }

        public async Task<ActionResult> TokenRequest(int productID, string state, string authorisationCode)
        {
            using (var ctx = new ProductDB())
            {
                var app = ctx.SSO_Applications.SingleOrDefault(a => a.ID == productID);
                if (app == null) throw new System.Exception("There is no product with an id of " + productID);

                // Ask the other product to exchange our authorisation code for an access token
                var tokenExchange = new SSO_TokenExchange()
                {
                    client_id = app.ClientID,
                    client_secret = app.ClientSecret,
                    code = authorisationCode,
                    scope = User.Identity.Name,
                    state = state,
                    grant_type = "authorization_code"
                };

                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(app.URL);

                var httpResponseMessage = await httpClient.PostAsJsonAsync("/oauth/token", tokenExchange);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponseMessage.ReasonPhrase);
                }
                else
                {
                    var result = await httpResponseMessage.Content.ReadAsAsync<SSO_TokenResponse>();
                    return Content(app.URL + "/oauth/signin?token=" + result.access_token);
                }
                
            }



        }

    }
}