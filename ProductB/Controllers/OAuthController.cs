using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProductB.Models;
using ProductB.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductB.Controllers
{
    public class OAuthController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
                {
                    // Store anything we will need later into session
                    Session["SSO_ClientID"] = client_id;
                    Session["SSO_SourceUserID"] = scope;
                    Session["SSO_State"] = state;
                    Session["SSO_RedirectURI"] = redirect_uri;

                    return View("SelectUser", "");
                }

                // Generate and store an authorisation code
                var authorisationCode = new SSO_AuthorisationCode()
                {
                    Code = Guid.NewGuid().ToString(),
                    LocalUserID = user.LocalUserID,
                    SourceUserID = user.SourceUserID,
                    State = state,
                    Timestamp = DateTime.Now,
                    ExistingLink = true
                };
                app.AuthorisationCodes.Add(authorisationCode);
                await ctx.SaveChangesAsync();

                // Redirect them
                return Redirect(redirect_uri + $"?state={state}&authorisationCode={authorisationCode.Code}");

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SelectUser(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var ok = SignInManager.UserManager.CheckPassword(user, model.Password);
            if (!ok)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            using (var ctx = new ProductDB())
            {

                // Fetch anything we need back out of session
                var client_id = Session["SSO_ClientID"] as string;
                var sourceUserID = Session["SSO_SourceUserID"] as string;
                var state = Session["SSO_State"] as string;
                var redirect_uri = Session["SSO_RedirectURI"] as string;

                // Find the details of this client
                var app = ctx.SSO_Applications.SingleOrDefault(a => a.ClientID == client_id && a.IsServer);
                if (app == null) throw new System.Exception("There is no client with an id of " + client_id);

                // Generate and store an authorisation code
                var authorisationCode = new SSO_AuthorisationCode()
                {
                    Code = Guid.NewGuid().ToString(),
                    LocalUserID = user.Email,
                    SourceUserID = sourceUserID,
                    State = state,
                    Timestamp = DateTime.Now,
                    ExistingLink = false
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

                //Store the link between the two applications
                if (!authCode.ExistingLink)
                {
                    app.ApplicationUsers.Add(new SSO_ApplicationUser()
                    {
                        LocalUserID = authCode.LocalUserID,
                        SourceUserID = authCode.SourceUserID
                    });
                }

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


        /// <summary>
        /// Final part of the oAuth sign.   There is a route which redirects OAuth/Signin to here
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(string token)
        {
            using (var ctx = new ProductDB())
            {
                var authCode = ctx.SSO_AccessTokens.SingleOrDefault(ac => ac.Token == token);
                if (authCode == null) throw new Exception("Invalid access code");

                //TODO: Check the token hasn't expired

                var user = await UserManager.FindByNameAsync(authCode.LocalUserID);

                if (user != null)
                {
                    await SignInManager.SignInAsync(user, true, true);
                }

                return RedirectToLocal("");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}