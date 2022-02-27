using CastingProject.Data;
using CastingProject.Filter;
using CastingProject.Helper;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CastingProject.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
        Random random = new Random();
        public ArtistController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
            this.context = context;
        }

        public IActionResult Index(GeneralFilter filter, int? page)
        {
            ViewBag.Gender = filter.Gender;
            ViewBag.SkinColor = filter.SkinColor;
            ViewBag.Height = filter.Height;
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            ViewBag.Category = context.Categories.ToList();

            var query = context.Artists.Include(x => x.Ethnicity).AsQueryable();
            if (!String.IsNullOrEmpty(filter.searchText))
            {
                query = query.Where(s => s.Name.ToLower().Contains(filter.searchText.ToLower()));
                ViewData["CurrentFilter"] = filter.searchText;
            }
            if (filter.Gender is not null)
            {
                query = query.Where(x => x.Gender == filter.Gender);
            }
            if (filter.SkinColor is not null)
            {
                query = query.Where(x => x.Skin_Color == filter.SkinColor);
            }
            if (filter.Ethnicity is not null)
            {
                query = query.Where(x => x.Ethnicity.Name == filter.Ethnicity);
            }
            if (filter.Category is not null)
            {
                query = query.Where(x => x.Category.Name == filter.Category);
            }
            if (filter.Height is not null)
            {
                var height = filter.Height.Split("-");

                int from = Convert.ToInt32(height[0]);
                int to = Convert.ToInt32(height[1]);
                query = query.Where(x => x.Height >= from && x.Height <= to);
            }
            int pageSize = 8;
            return View(PaginatedList<Artist>.CreateAsync(query.AsNoTracking(), page ?? 1, pageSize));


            //ViewBag.Artist = query.ToList();
        }

        public IActionResult Create()
        {
            var artist = new Artist();
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            ViewBag.Category = context.Categories.ToList();
            ViewBag.Role = context.Roles.ToList();
            ViewBag.Hobby = context.Hobbies.ToList();
            ViewBag.Skill = context.Skills.ToList();
            return View(artist);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Artist artist, int[] SelectedRoleId, int[] SelectedHobbyId, int[] SelectedSkillID)
        {
            try
            {
                ViewData["Ethnicity"] = context.Ethnicities.ToList();
                if (ModelState.IsValid)
                {
                    IFormFile postedFile;
                    string artistName = artist.Name.ToString();
                    //uploading Dp
                    if (artist.ImageFile != null)
                    {
                        postedFile = artist.ImageFile;
                        string extension = Path.GetExtension(postedFile.FileName);
                        if (FileValid(postedFile, extension))
                        {
                            var imagePath = UploadFile(artistName, postedFile);
                            artist.Dp = imagePath;
                        }
                        else
                        {
                            TempData["message.filevalidation"] = "File not valid";
                            return View();
                        }
                    }
                    if (artist.GalleryFile != null)
                    {
                        foreach (var item in artist.GalleryFile)
                        {
                            postedFile = item;
                            string extension = Path.GetExtension(postedFile.FileName);
                            if (FileValid(postedFile, extension))
                            {
                                var imagePath = UploadFile(artistName, postedFile);
                                artist.ArtistGalleries.Add(new ArtistGallery { Gallery = imagePath });
                            }
                            else
                            {
                                TempData["message.filevalidation"] = "File not valid";
                                return View();
                            }
                        }
                    }
                    if (SelectedHobbyId != null)
                    {
                        foreach (var hobbyId in SelectedHobbyId)
                        {
                            artist.ArtistHobbies.Add(new ArtistHobbies { HobbyId = hobbyId });
                        }
                    }

                    if (SelectedRoleId != null)
                    {
                        foreach (var roleId in SelectedRoleId)
                        {
                            artist.ArtistRoles.Add(new ArtistRole { RoleId = roleId });
                        }
                    }

                    if (SelectedSkillID != null)
                    {
                        foreach (var skillId in SelectedSkillID)
                        {
                            artist.ArtistSkills.Add(new ArtistSkills { SkillId = skillId });
                        }
                    }
                    context.Artists.Add(artist);
                    context.SaveChanges();
                }
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        //Details
        public IActionResult Details(int id)
        {
            var artist = context.Artists.Include(x => x.ArtistGalleries).Include(x=>x.Category).FirstOrDefault(x => x.Id == id);

            return View(artist);
        }

        //Edit
        public IActionResult Edit(int id)
        {
            var artist = context.Artists.Include(x=>x.ArtistRoles).Include(x=>x.ArtistHobbies).Include(x=>x.ArtistSkills).FirstOrDefault(x => x.Id == id);
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            ViewBag.Category = context.Categories.ToList();
            ViewBag.Role = context.Roles.ToList();
            ViewBag.Hobby = context.Hobbies.ToList();
            ViewBag.Skill = context.Skills.ToList();
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Artist artist, int[] SelectedRoleId, int[] SelectedHobbyId, int[] SelectedSkillId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldArtist = context.Artists.Include(x=>x.ArtistRoles).Include(x=>x.ArtistHobbies).Include(x=>x.ArtistSkills).FirstOrDefault(x=>x.Id == id);
                    IFormFile postedFile;
                    string artistName = artist.Name.ToString();
                    if (oldArtist != null)
                    {
                        if (artist.ImageFile != null)
                        {
                            postedFile = artist.ImageFile;
                            string extension = Path.GetExtension(postedFile.FileName);
                            if (FileValid(postedFile, extension))
                            {
                                var imagePath = UploadFile(artistName, postedFile);
                                artist.Dp = imagePath;
                            }
                            else
                            {
                                TempData["message.filevalidation"] = "File not valid";
                                return View(oldArtist);
                            }
                            if (oldArtist.Dp != null)
                            {
                                string oldImagePath = Path.Combine(hostEnvironment.WebRootPath, oldArtist.Dp);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }
                            oldArtist.Dp = artist.Dp;
                        }

                        //----------Artist Role---------- 
                        if (SelectedRoleId != null)
                        {
                            //Remove Artist Role
                            var removedArtistRole = new List<ArtistRole>();
                            foreach (var artistRoleId in oldArtist.ArtistRoles)
                            {
                                if (!SelectedRoleId.Contains(artistRoleId.RoleId))
                                    removedArtistRole.Add(artistRoleId);
                            }

                            //removing old Roles
                            foreach (var artistRoleId in removedArtistRole)
                            {
                                context.ArtistRoles.Remove(artistRoleId);
                            }

                            //add newly selected Roles
                            foreach (var selectedRoleId in SelectedRoleId)
                            {
                                if (!oldArtist.ArtistRoles.Any(x=>x.RoleId == selectedRoleId))
                                {
                                    context.ArtistRoles.Add(new ArtistRole { RoleId = selectedRoleId, ArtistId = artist.Id });
                                }
                            }
                        }

                        //Artist Hobby
                        if (SelectedHobbyId != null)
                        {
                            //Remove Artis Hobby
                            var removedArtistHobby = new List<ArtistHobbies>();
                            foreach (var artistHobbyId in oldArtist.ArtistHobbies)
                            {
                                if (!SelectedHobbyId.Contains(artistHobbyId.HobbyId))
                                {
                                    removedArtistHobby.Add(artistHobbyId);
                                }
                            }

                            //removing old Hobbies
                            foreach (var artistHobbyId in removedArtistHobby)
                            {
                                context.ArtistHobbies.Remove(artistHobbyId);
                            }

                            //add newly selected Roles
                            foreach (var selectedHobbyId in SelectedHobbyId)
                            {
                                if (!oldArtist.ArtistHobbies.Any(x => x.HobbyId == selectedHobbyId))
                                {
                                    context.ArtistHobbies.Add(new ArtistHobbies { HobbyId = selectedHobbyId, ArtistId = artist.Id });
                                }
                            }

                        }

                        //Artist Skill
                        if (SelectedSkillId != null)
                        {
                            //Remove Artis Hobby
                            var removedArtistSkill = new List<ArtistSkills>();
                            foreach (var artistSkillId in oldArtist.ArtistSkills)
                            {
                                if (!SelectedSkillId.Contains(artistSkillId.SkillId))
                                {
                                    removedArtistSkill.Add(artistSkillId);
                                }
                            }

                            //removing old Hobbies
                            foreach (var artistSkillId in removedArtistSkill)
                            {
                                context.ArtistSkills.Remove(artistSkillId);
                            }

                            //add newly selected Roles
                            foreach (var selectedSkillId in SelectedSkillId)
                            {
                                if (!oldArtist.ArtistSkills.Any(x => x.SkillId == selectedSkillId))
                                {
                                    context.ArtistSkills.Add(new ArtistSkills { SkillId = selectedSkillId, ArtistId = artist.Id });
                                }
                            }

                        }
                        oldArtist.Name = artist.Name;
                        oldArtist.Gender = artist.Gender;
                        oldArtist.EthnicityId = artist.EthnicityId;
                        oldArtist.CategoryId = artist.CategoryId;
                        oldArtist.Height = artist.Height;
                        oldArtist.Weight = artist.Weight;
                        oldArtist.Skin_Color = artist.Skin_Color;
                        oldArtist.Eye_Color = artist.Eye_Color;
                        oldArtist.Hair_Color = artist.Hair_Color;
                        oldArtist.Skin_Type = artist.Skin_Type;
                        oldArtist.Hair_Length = artist.Hair_Length;
                        oldArtist.Hair_Type = artist.Hair_Type;
                        oldArtist.DOB = artist.DOB;
                    }
                }
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /*   public IActionResult Delete(int id)
           {
               var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
               return View(artist);
           }
    */
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
                var gallery = artist.ArtistGalleries.Where(x => x.ArtistId == id);
                if (artist.Dp != null)
                {
                    string imagePath = Path.Combine(hostEnvironment.WebRootPath, artist.Dp);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                foreach (var item in gallery)
                {
                    if (item.Gallery != null)
                    {
                        string imagePath = Path.Combine(hostEnvironment.WebRootPath, item.Gallery);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                }
                context.Artists.Remove(artist);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult GalleryIndex(int id)
        {
            var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
            // var artistGallery=artist.ArtistGalleries.Where(x=>x.ArtistId==id);
            return View(artist);
            //return PartialView("_GalleryPartialView",artist);
        }

        public IActionResult GalleryUpdate(int id)
        {
            var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
            //return View(artist);
            return PartialView("_GalleryUpdate", artist);

        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult GalleryUpdate(int id, Artist artist)
        {
            // if (ModelState.IsValid)
            // {
            var newArtist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
            if (artist.GalleryFile != null)
            {
                foreach (var item in artist.GalleryFile)
                {
                    IFormFile postedFile;
                    string artistName = artist.Name.ToString();
                    postedFile = item;
                    string extension = Path.GetExtension(postedFile.FileName);
                    if (FileValid(postedFile, extension))
                    {
                        var imagePath = UploadFile(artistName, postedFile);
                        context.ArtistGalleries.Add(new ArtistGallery { ArtistId = id, Gallery = imagePath });
                    }
                    else
                    {
                        TempData["message.filevalidation"] = "File not valid";
                        return View();
                    }
                }
            }
            newArtist = artist;
            context.SaveChanges();
            return RedirectToAction(nameof(GalleryIndex), new { Id = id });
            return PartialView("_GalleryUpdate", newArtist);
            // }
        }


        public IActionResult DeleteGallery(int id)
        {
            var artistGallery = context.ArtistGalleries.Find(id);
            var artistId = artistGallery.ArtistId;
            if (artistGallery.Gallery != null)
            {
                string imagePath = Path.Combine(hostEnvironment.WebRootPath, artistGallery.Gallery);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            context.ArtistGalleries.Remove(artistGallery);
            context.SaveChanges();
            return RedirectToAction(nameof(GalleryIndex), new { id = artistId });
        }

        //File Validation Function
        private bool FileValid(IFormFile file, string extension)
        {
            long length = file.Length;

            if (permittedExtensions.Contains(extension))
            {
                if (length <= 100000)
                {
                    return true;
                }
                return true;
            }

            return false;
        }

        //File Upload Function
        private string UploadFile(string artistName, IFormFile file)
        {
            string imgPath = "images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/";
            string directory = hostEnvironment.WebRootPath + "/" + imgPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var image = "";
            IFormFile postedFile = file;
            string filename = artistName;
            filename = filename + " " + random.Next(1000, 9999);
            filename = SlugHelper.GenerateSlug(filename);
            string extension = Path.GetExtension(postedFile.FileName);
            filename = filename + extension;
            image = imgPath + filename;

            string path = Path.Combine(directory, filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                postedFile.CopyTo(fileStream);
            }

            return image;
        }
    }
}
