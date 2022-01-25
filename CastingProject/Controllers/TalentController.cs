using CastingProject.Data;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Controllers
{
    public class TalentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TalentController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        // GET: TalentController
        public ActionResult Index(string Search_Data)
        {
            var talent = from _talent in _context.talents select _talent;
            if (!String.IsNullOrEmpty(Search_Data))
            {
                talent = talent.Where(_talent => _talent.Name.ToLower().Contains(Search_Data.ToLower()));
            }
            return View(talent.ToList());
        }

        // GET: TalentController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var talent = _context.talents.Find(id);
            return View(talent);
        }

        // GET: TalentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TalentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Talent talent)
        {
            try
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(talent.ImageFile.FileName);
                string extension = Path.GetExtension(talent.ImageFile.FileName);
                IFormFile postedFile = talent.ImageFile;
                long length = postedFile.Length;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                {
                    if (length <= 1000000)
                    {
                        talent.Profile = filename = filename + extension;
                        string path = Path.Combine(webRootPath + "/Profile/" + filename);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            talent.ImageFile.CopyTo(fileStream);
                        }
                        //Insert Record
                        _context.talents.Add(talent);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TalentController/Edit/5
        public ActionResult Edit(int id)
        {
            var talent = _context.talents.Find(id);
            return View(talent);
        }

        // POST: TalentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Talent talent)
        {
            try
            {
                if (talent.ImageFile != null)
                {
                    var oldTalent = _context.talents.Find(id);
                    string webRootPath = _hostEnvironment.WebRootPath;
                    string filename = Path.GetFileNameWithoutExtension(talent.ImageFile.FileName);
                    string extension = Path.GetExtension(talent.ImageFile.FileName);
                    IFormFile postedFile = talent.ImageFile;
                    long length = postedFile.Length;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {
                        if (length <= 1000000)
                        {
                            talent.Profile = filename = filename + extension;
                            string path = Path.Combine(webRootPath + "/Profile/" + filename);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                talent.ImageFile.CopyTo(fileStream);
                            }
                            /* _context.Entry(talent).State = EntityState.Modified;*/
                            if (oldTalent.Profile != null)
                            {
                                string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Profile/", oldTalent.Profile);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }
                            /*     oldTalent.Name = talent.Name;
                                 oldTalent.Age = talent.Age;*/

                            oldTalent.Profile = talent.Profile;
                            _context.SaveChanges();

                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Entry(talent).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }
            }
            catch
            {
                return View();
            }
        }

        // GET: TalentController/Delete/5
        public ActionResult Delete(int id)
        {
            var talent = _context.talents.Find(id);
            return View(talent);
        }

        // POST: TalentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var talent = _context.talents.Find(id);
                _context.talents.Remove(talent);
                if (talent.Profile != null)
                {
                    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Profile/", talent.Profile);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
