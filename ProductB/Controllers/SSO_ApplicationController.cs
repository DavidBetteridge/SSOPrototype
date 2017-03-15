using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductB;
using ProductB.Models;

namespace ProductB.Controllers
{
    public class SSO_ApplicationController : Controller
    {
        private ProductDB db = new ProductDB();

        // GET: SSO_Application
        public async Task<ActionResult> Index()
        {
            return View(await db.SSO_Applications.Where(a => a.IsServer).ToListAsync());
        }

        // GET: SSO_Application/Details/5
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

        // GET: SSO_Application/Create
        public ActionResult Create()
        {
            var model = new SSO_Application()
            {
                IsServer = true,
                ClientID = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString()
            };

            return View(model);
        }

        // POST: SSO_Application/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,ProductType,URL,ClientID,ClientSecret")] SSO_Application sSO_Application)
        {
            if (ModelState.IsValid)
            {
                sSO_Application.IsServer = true;
                db.SSO_Applications.Add(sSO_Application);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sSO_Application);
        }

        // GET: SSO_Application/Edit/5
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

        // POST: SSO_Application/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,ProductType,URL,ClientID,ClientSecret")] SSO_Application sSO_Application)
        {
            if (ModelState.IsValid)
            {
                sSO_Application.IsServer = true;
                db.Entry(sSO_Application).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sSO_Application);
        }

        // GET: SSO_Application/Delete/5
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

        // POST: SSO_Application/Delete/5
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
