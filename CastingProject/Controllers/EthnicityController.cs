using CastingProject.Data;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CastingProject.Controllers
{
    public class EthnicityController : Controller
    {
        private readonly ApplicationDbContext context;

        public EthnicityController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // GET: EthnicityController
        public ActionResult Index()
        {
            var ethnicities = context.Ethnicities.ToList();
            return View(ethnicities);
        }

        // GET: EthnicityController/Details/5
        public ActionResult Details(int id)
        {
            var ethnicity = context.Ethnicities.Find(id);
            return View(ethnicity);
        }

        // GET: EthnicityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EthnicityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ethnicity ethnicity)
        {
            try
            {
                context.Ethnicities.Add(ethnicity);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EthnicityController/Edit/5
        public ActionResult Edit(int id)
        {
            var ethnicity = context.Ethnicities.Find(id);
            return View(ethnicity);
        }

        // POST: EthnicityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Ethnicity ethnicity)
        {
            try
            {
                var _ethnicity = context.Ethnicities.Find(id);
                _ethnicity.Name = ethnicity.Name;
                context.Ethnicities.Update(_ethnicity);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EthnicityController/Delete/5
        public ActionResult Delete(int id)
        {
            var ethnicity = context.Ethnicities.Find(id);
            return View(ethnicity);
        }

        // POST: EthnicityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var ethnicity = context.Ethnicities.Find(id);
                context.Ethnicities.Remove(ethnicity);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
