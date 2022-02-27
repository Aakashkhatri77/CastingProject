using CastingProject.Data;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CastingProject.Controllers
{
    public class HobbyController : Controller
    {
        private readonly ApplicationDbContext context;

        public HobbyController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: HobbyController
        public ActionResult Index()
        {
            var hobbies = context.Hobbies.ToList(); 
            return View(hobbies);
        }

        // GET: HobbyController/Details/5
        public ActionResult Details(int id)
        {
            var hobbies = context.Hobbies.Find(id); 
            return View(hobbies);
        }

        // GET: HobbyController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: HobbyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hobby hobby)
        {
            try
            {
                context.Hobbies.Add(hobby);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HobbyController/Edit/5
        public ActionResult Edit(int id)
        {
            var hobbies = context.Hobbies.Find(id);
            return View(hobbies);
        }

        // POST: HobbyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Hobby hobby)
        {
            try
            {
                var _hobby = context.Hobbies.Find(id);
                _hobby.Name = hobby.Name;
                context.Hobbies.Update(_hobby);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HobbyController/Delete/5
        public ActionResult Delete(int id)
        {
            var hobbies = context.Hobbies.Find(id);
            return View(hobbies);
        }

        // POST: HobbyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var hobbies = context.Hobbies.Find(id);
                context.Hobbies.Remove(hobbies);
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
