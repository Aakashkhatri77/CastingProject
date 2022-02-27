using CastingProject.Data;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CastingProject.Controllers
{
    public class SkillController : Controller
    {
        private readonly ApplicationDbContext context;

        public SkillController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            var skills = context.Skills.ToList();
            return View(skills);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var skill = context.Skills.Find(id);
            return View(skill);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Skill skill)
        {
            try
            {
                context.Skills.Add(skill);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var skill = context.Skills.Find(id);
            return View(skill);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Skill skill)
        {
            try
            {
                var _skill = context.Skills.Find(id);
                _skill.Name = skill.Name;
                context.Skills.Update(_skill);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var skill = context.Skills.Find(id);
            return View(skill);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var skill = context.Skills.Find(id);
                context.Skills.Remove(skill);
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
