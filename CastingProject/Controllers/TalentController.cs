using CastingProject.Data;
using CastingProject.Helper;
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
        Random random = new Random();

        public TalentController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
           
        }
        // GET: TalentController
        public ActionResult Index(string Search_Data)
        {
            var talent = from _talent in _context.talents.Include(tp => tp.talentProfiles) select _talent;
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
            var talent = _context.talents.Include(tp => tp.talentProfiles).FirstOrDefault(x => x.Id == id);
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
                string imagePath= "Profile/" + DateTime.Now.ToString("yyyy")+ "/" +DateTime.Now.ToString("MM") + "/";
                string directory = _hostEnvironment.WebRootPath + "/" + imagePath ;
                if (!Directory.Exists(directory)) 
                {
                   Directory.CreateDirectory(directory);
                }
                foreach (var item in talent.ImageFile)
                {
                    //string webRootPath = _hostEnvironment.WebRootPath;
                    string filename = talent.Name.ToString();
                    filename = filename+" "+random.Next(1000,9999);
                    filename = SlugHelper.GenerateSlug(filename);
                    string extension = Path.GetExtension(item.FileName);
                    IFormFile postedFile = item;
                    long length = postedFile.Length;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {
                        if (length <= 1000000)
                        {
                            filename = filename + extension;
                            
                            var profile = imagePath + filename;
                            string path = Path.Combine(directory , filename);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                item.CopyTo(fileStream);
                            }
                            //Insert Record
                            talent.talentProfiles.Add(new TalentProfile { ProfileName = profile });
                        }
                    }
                }
                _context.talents.Add(talent);
                _context.SaveChanges();
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
                    foreach (var item in talent.ImageFile)
                    {
                        var oldTalent = _context.talents.Find(id);
                        string webRootPath = _hostEnvironment.WebRootPath;
                        string filename = Path.GetFileNameWithoutExtension(item.FileName);
                        string extension = Path.GetExtension(item.FileName);
                        IFormFile postedFile = item;
                        long length = postedFile.Length;
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                        {
                            if (length <= 1000000)
                            {
                                var Profile = filename = filename + extension;
                                string path = Path.Combine(webRootPath + "/Profile/" + filename);
                                using (var fileStream = new FileStream(path, FileMode.Create))
                                {
                                    item.CopyTo(fileStream);
                                }
                                /* _context.Entry(talent).State = EntityState.Modified;*/

                                /*      if (oldTalent.Profile != null)
                                      {
                                          string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Profile/", oldTalent.Profile);
                                          if (System.IO.File.Exists(imagePath))
                                          {
                                              System.IO.File.Delete(imagePath);
                                          }
                                      }*/
                                oldTalent.talentProfiles = talent.talentProfiles;
                                _context.SaveChanges();

                            }
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
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: TalentController/Delete/5
        public ActionResult Delete(int id)
        {
            var talent = _context.talents.Include(tp => tp.talentProfiles).FirstOrDefault(x => x.Id == id);
            return View(talent);
        }

        // POST: TalentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var talent = _context.talents.Include(tp => tp.talentProfiles).FirstOrDefault(x => x.Id == id);
                var profiles = talent.talentProfiles.Where(x => x.TalentId == id);
                foreach (var item in profiles)
                {
                    if (item.ProfileName != null)
                    {
                        string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Profile/", item.ProfileName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                }
                _context.talents.Remove(talent);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult EditGallery(int id)
        {
            var talent = _context.talents.Include(tp => tp.talentProfiles).FirstOrDefault(x => x.Id == id);
            var talentPicture = talent.talentProfiles.Where(x => x.TalentId == id);
            return View(talentPicture);
        }

        public IActionResult DeleteGallery(int id)
        {
            var talentPicture = _context.talentProfiles.Find(id);
            var TalentId = talentPicture.TalentId;
            if (talentPicture.ProfileName != null)
            {
                string imagePath = Path.Combine(_hostEnvironment.WebRootPath, talentPicture.ProfileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.talentProfiles.Remove(talentPicture);
            _context.SaveChanges();
            return RedirectToAction(nameof(EditGallery), new { id = TalentId });
        }
    }
}
