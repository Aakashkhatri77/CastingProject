using CastingProject.Data;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CastingProject.Controllers
{
    public class RolesController : Controller
    {
        public readonly ApplicationDbContext context;

        public RolesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: RolesController
        public ActionResult Index()
        {
            var role = context.Roles.ToList();
            return View(role);
        }

        // GET: RolesController/Details/5
        public ActionResult Details(int id)
        {
            var role = context.Roles.Find(id);
            return View(role) ;
        }

        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            try
            {
                context.Roles.Add(role);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RolesController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = context.Roles.Find(id);
            return View(role);
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Role role)
        {
            try
            {
                var _role = context.Roles.Find(id);
                context.Roles.Update(_role);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RolesController/Delete/5
        public ActionResult Delete(int id)
        {
            var role = context.Roles.Find(id);
            return View(role);
        }

        // POST: RolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var role = context.Roles.Find(id);
                context.Roles.Remove(role);
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
