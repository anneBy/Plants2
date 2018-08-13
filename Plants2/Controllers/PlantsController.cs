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
        public ActionResult Index(string searchString)
        {
            var plants = db.Plants.Include(p => p.Parent).Include(p => p.Commonnames);

            if (!String.IsNullOrEmpty(searchString))
            {
                plants = plants.Where(p => p.Name.Contains(searchString));
            }

            plants = plants.OrderBy(p => p.Name);
            return View(plants.ToList());
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
            ViewBag.ParentID = new SelectList(db.Plants, "PlantID", "Name");
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlantID,Name,ParentID,HLevelName,NativTo")] Plant plant)
        {
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

            Plant plant = db.Plants
              .Include(p => p.Commonnames)
              .SingleOrDefault(x => x.PlantID == id);

            if (plant == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentID = new SelectList(db.Plants, "PlantID", "Name", plant.ParentID);
            ViewBag.CommonNms = new SelectList(plant.Commonnames, "CommID", "CommName");
            return View(plant);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlantID,Name,ParentID,HLevelName,NativTo")] Plant plant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Plants, "PlantID", "Name", plant.ParentID);
            return View(plant);
        }

        // GET: Partial View Plants/AddCommonname
        public PartialViewResult AddCommonname(string plId)
        {
            int pid = -1;
            string message = "";
            Plant plant = new Plant();
            List<Commonname> allExceptPlantsCn = new List<Commonname>();

            if (int.TryParse(plId, out pid))
            {
                plant = db.Plants
               .Include(p => p.Commonnames)
               .SingleOrDefault(x => x.PlantID == pid);

                if (plant != null)
                {
                    allExceptPlantsCn = db.Commonnames.OrderBy(c => c.CommName).ToList().Except(plant.Commonnames.ToList()).ToList();
                }
                else
                {
                    plant = new Plant();
                    message = "Sorry, could not find requested Plant.";
                }
            }
            else
            {
                message = "Sorry, error while parsing requested Plant.";
            }

            ViewBag.AllCommonNms = new SelectList(allExceptPlantsCn, "CommID", "CommName");
            ViewBag.Message = message;
            return PartialView("AddCommonname", plant);
        }

        public ActionResult addCommName(string commId, string plantId)
        {
            int cid = 0, pid = 0;
            Plant plant = null;
            List<Commonname> addedCommName = new List<Commonname>();

            if (int.TryParse(plantId, out pid))
            {
                plant = db.Plants.Include("Commonnames")
                    .Where(p => p.PlantID == pid)
                    .FirstOrDefault<Plant>();
            }
            else
            {
                return HttpNotFound();
            }
            if (int.TryParse(commId, out cid))
            {
                addedCommName = db.Commonnames.Where(c => c.CommID == cid).ToList();
            }
            else
            {
                return HttpNotFound();
            }

            foreach (Commonname c in addedCommName)
            {
                if (db.Entry(plant).State == EntityState.Detached)
                    db.Commonnames.Attach(c);
                plant.Commonnames.Add(c);
            }
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = pid });
        }

        public ActionResult removeCommName(string commId, string plantId)
        {
            int cid = 0, pid = 0;
            Plant plant = null;
            List<Commonname> deletedCommNames = new List<Commonname>();

            if (int.TryParse(plantId, out pid))
            {
                plant = db.Plants.Include("Commonnames")
                    .Where(p => p.PlantID == pid)
                    .FirstOrDefault<Plant>();
            }
            else
            {
                return HttpNotFound();
            }
            if (int.TryParse(commId, out cid))
            {
                deletedCommNames = plant.Commonnames.Except(plant.Commonnames
                    .Where(t => t.CommID != cid)).ToList();
            }
            else
            {
                return HttpNotFound();
            }

            deletedCommNames.ForEach(c => plant.Commonnames.Remove(c));
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = pid });
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
