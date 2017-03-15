using ProductB.Models;
using ProductB.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductB.Controllers
{
    public class OAuthController : Controller
    {
        // GET: SSO
        public async Task<ActionResult> Authorize(string state, string scope, string client_id, string redirect_uri, string response_type)
        {
            using (var ctx = new ProductDB())
            {
                // Find the details of this client
                var app = ctx.SSO_Applications.SingleOrDefault(a => a.ClientID == client_id && a.IsServer);
                if (app == null) throw new System.Exception("There is no client with an id of " + client_id);

                // Check we have been provided with a valid redirect_uri
                if (!redirect_uri.StartsWith(app.URL)) throw new System.Exception("Invalid redirect uri");

                // Do we know who this user is?
                var user = app.ApplicationUsers.SingleOrDefault(au => au.SourceUserID == scope);

                // No ask them to map
                if (user == null)
                    return View();


                // Generate and store an authorisation code
                var authorisationCode = new SSO_AuthorisationCode()
                {
                    Code = Guid.NewGuid().ToString(),
                    LocalUserID = user.LocalUserID,
                    SourceUserID = user.SourceUserID,
                    State = state,
                    Timestamp = DateTime.Now
                };
                app.AuthorisationCodes.Add(authorisationCode);
                await ctx.SaveChangesAsync();

                // Redirect them
                return Redirect(redirect_uri + $"?state={state}&authorisationCode={authorisationCode.Code}");

            }
        }

        [HttpPost]
        public async Task<ActionResult> Token(SSO_TokenExchange details)
        {
            using (var ctx = new ProductDB())
            {
                // Find the details of this client
                var app = ctx.SSO_Applications.SingleOrDefault(a => a.ClientID == details.client_id && a.IsServer);
                if (app == null) throw new System.Exception("There is no client with an id of " + details.client_id);

                // Check the passord
                if (app.ClientSecret != details.client_secret) throw new System.Exception("Invalid client secret");

                // Fetch the details of the authorisation code
                var authCode = ctx.SSO_AuthorisationCodes.SingleOrDefault(ac => ac.Code == details.code && ac.SourceUserID == details.scope && ac.State == details.state);
                // var authCode = ctx.SSO_AuthorisationCodes.First();
                if (authCode == null) throw new System.Exception("Invalid auth code");

                // Remove the auth code
                ctx.SSO_AuthorisationCodes.Remove(authCode);

                // Create a new access code
                var accessToken = new SSO_AccessToken()
                {
                    LocalUserID = authCode.LocalUserID,
                    Timestamp = DateTime.Now,
                    Token = Guid.NewGuid().ToString()
                };
                ctx.SSO_AccessTokens.Add(accessToken);


                //TODO: Create the user xref

                await ctx.SaveChangesAsync();

                return Json(new SSO_TokenResponse()
                {
                    access_token = accessToken.Token,
                    expires_id = "",
                    refresh_token = "",
                    scope = authCode.LocalUserID,
                    info = "",
                    token_type = "BEARER",
                    uid = accessToken.ID.ToString()
                }
                    );
            }
        }

    }
}