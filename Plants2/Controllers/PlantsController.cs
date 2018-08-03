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
    public class PlantsController : Controller
    {
        private Plant2DBContext db = new Plant2DBContext();

        // GET: Plants
        public ActionResult Index(string hLevelName, string searchString)
        {
            var LevelLst = new List<string>();

            var LevelQry = from d in db.Plants
                           orderby d.HLevelName
                           select d.HLevelName;

            LevelLst.AddRange(LevelQry.Distinct());
            ViewBag.hLevelName = new SelectList(LevelLst);

            var plants = from p in db.Plants
                         select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                plants = plants.Where(s => s.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(hLevelName))
            {
                plants = plants.Where(x => x.HLevelName == hLevelName);
            }

            plants = plants.OrderBy(p => p.Name);

            return View(plants);
        }

        // GET: Plants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }

            return View(plant);
        }

        // GET: Plants/Create
        public ActionResult Create()
        {
            var ParentLst = new List<string>();

            var ParentQry = from p in db.Plants
                            orderby p.Name
                            select p.Name;

            ParentLst.AddRange(ParentQry);
            ViewBag.parentNameList = new SelectList(ParentLst);

            var plant = new Plant();

            return View(plant);
        }

        // POST: Plants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlantID,Name,ParentID,HLevelName")] Plant plant, string parentNameList)
        {
            if (!string.IsNullOrEmpty(parentNameList))
            {
                var plntIdQuery =
                    (from p in db.Plants
                     where p.Name == parentNameList
                     select new { p.PlantID }).Single(); // .Single() is like executeScalar(), Sequence must not contain more than one element, otherwise exception is trown.

                int id = plntIdQuery.PlantID;
                plant.ParentID = id;
            }

            if (ModelState.IsValid)
            {
                db.Plants.Add(plant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(db.Plants, "PlantID", "Name", plant.ParentID);
            return View(plant);
        }

        // GET: Plants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentID = new SelectList(db.Plants, "PlantID", "Name", plant.ParentID);    // (items, data value field, data text field, selected value)
            return View(plant);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlantID,Name,ParentID,HLevelName")] Plant plant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plant);
        }

        // GET: Plants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plant plant = db.Plants.Find(id);
            db.Plants.Remove(plant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Plants/UnderConstruction
        public ActionResult UnderConstruction()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsPlantInDB(string Name, int? PlantID)
        {
            //check if any of the Plant Names in db matches the Name specified in the Parameter using the ANY extension method. 
            //PlantID check to allow edit 
            return Json(!db.Plants.Any(x => x.Name == Name && x.PlantID != PlantID), JsonRequestBehavior.AllowGet);
        }
    }
}
