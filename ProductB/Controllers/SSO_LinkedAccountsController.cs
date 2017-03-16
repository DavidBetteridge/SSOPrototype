using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using ProductB.Models;

namespace ProductB.Controllers
{
    public class SSO_LinkedAccountsController : Controller
    {
        private ProductDB db = new ProductDB();

        // GET: SSO_LinkedAccounts
        public async Task<ActionResult> Index()
        {
            var allApplications = await db.SSO_Applications.ToListAsync();
            var models = await db.SSO_ApplicationUsers.Where(a => a.LocalUserID == User.Identity.Name).ToListAsync();
            var viewModels = models.Select(m => new ProductB.ViewModels.LinkedAccount()
            {
                AccountName = m.SourceUserID,
                ProductName = allApplications.Single(a => a.ID == m.SSO_ApplicationID).Name,
                ID = m.ID
            });

            return View(viewModels);
        }


        // GET: SSO_LinkedAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.SSO_ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.LocalUserID != User.Identity.Name) return HttpNotFound();

            var application = await db.SSO_Applications.FindAsync(user.SSO_ApplicationID);

            var viewModel = new ProductB.ViewModels.LinkedAccount()
            {
                AccountName = user.SourceUserID,
                ProductName = application.Name,
                ID = user.ID
            };

            return View(viewModel);
        }

        // POST: SSO_LinkedAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SSO_ApplicationUser sSO_ApplicationUser = await db.SSO_ApplicationUsers.FindAsync(id);
            if (sSO_ApplicationUser.LocalUserID != User.Identity.Name) return HttpNotFound();
            db.SSO_ApplicationUsers.Remove(sSO_ApplicationUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
