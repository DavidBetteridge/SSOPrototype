using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductA;
using ProductA.Models;

namespace ProductA.Controllers
{
    public class RegisteredApplicationController : Controller
    {
        private ProductDB db = new ProductDB();

        public ActionResult Render()
        {
            return View(db.SSO_Applications.Where(a => !a.IsServer).ToList());
        }

        // GET: RegisteredApplication
        public async Task<ActionResult> Index()
        {
            return View(await db.SSO_Applications.Where(a => !a.IsServer).ToListAsync());
        }

        // GET: RegisteredApplication/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSO_Application sSO_Application = await db.SSO_Applications.FindAsync(id);
            if (sSO_Application == null)
            {
                return HttpNotFound();
            }
            return View(sSO_Application);
        }

        // GET: RegisteredApplication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisteredApplication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,ProductType,URL,ClientID,ClientSecret")] SSO_Application sSO_Application)
        {
            if (ModelState.IsValid)
            {
                sSO_Application.IsServer = false;
                db.SSO_Applications.Add(sSO_Application);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sSO_Application);
        }

        // GET: RegisteredApplication/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSO_Application sSO_Application = await db.SSO_Applications.FindAsync(id);
            if (sSO_Application == null)
            {
                return HttpNotFound();
            }
            return View(sSO_Application);
        }

        // POST: RegisteredApplication/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,ProductType,URL,ClientID,ClientSecret")] SSO_Application sSO_Application)
        {
            if (ModelState.IsValid)
            {
                sSO_Application.IsServer = false;
                db.Entry(sSO_Application).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sSO_Application);
        }

        // GET: RegisteredApplication/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSO_Application sSO_Application = await db.SSO_Applications.FindAsync(id);
            if (sSO_Application == null)
            {
                return HttpNotFound();
            }
            return View(sSO_Application);
        }

        // POST: RegisteredApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SSO_Application sSO_Application = await db.SSO_Applications.FindAsync(id);
            db.SSO_Applications.Remove(sSO_Application);
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
