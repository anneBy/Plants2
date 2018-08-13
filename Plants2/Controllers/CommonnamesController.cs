using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plants2.Models;

namespace Plants2.Controllers
{
    public class CommonnamesController : Controller
    {
        private Plant2DBContext db = new Plant2DBContext();

        // GET: Commonnames
        public ActionResult Index()
        {
            return View(db.Commonnames.OrderBy(c => c.CommName).ToList());
        }

        // GET: Commonnames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commonname commonname = db.Commonnames.Find(id);
            if (commonname == null)
            {
                return HttpNotFound();
            }
            return View(commonname);
        }

        // GET: Commonnames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commonnames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommID,CommName,Language")] Commonname commonname)
        {
            if (ModelState.IsValid)
            {
                db.Commonnames.Add(commonname);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(commonname);
        }

        // GET: Commonnames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commonname commonname = db.Commonnames.Find(id);
            if (commonname == null)
            {
                return HttpNotFound();
            }
            return View(commonname);
        }

        // POST: Commonnames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommID,CommName,Language")] Commonname commonname)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commonname).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commonname);
        }

        // GET: Commonnames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commonname commonname = db.Commonnames.Find(id);
            if (commonname == null)
            {
                return HttpNotFound();
            }
            return View(commonname);
        }

        // POST: Commonnames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commonname commonname = db.Commonnames.Find(id);
            db.Commonnames.Remove(commonname);
            db.SaveChanges();
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
