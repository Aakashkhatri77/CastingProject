using CastingProject.Data;
using CastingProject.Filter;
using CastingProject.Helper;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace CastingProject.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
        Random random = new Random();
        private string image;
        private const int ThumbnailWidth = 150;
        private const int MediumWidth = 400;
        private const int FullScreenWidth = 1400;
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
            ViewBag.Filter = filter;

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

            if (filter.Age is not null)
            {
                var age = filter.Age.Split("-");
                int ageFrom = Convert.ToInt32(age[0]);
                int ageTo = Convert.ToInt32(age[1]);
                var ageFromDate = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - ageTo).ToString() + "-1-1";
                var ageToDate = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - ageFrom).ToString() + "-12-31";
                DateTime From = DateTime.Parse(ageFromDate);
                DateTime To = DateTime.Parse(ageToDate);
                query = query.Where(x => x.DOB.Date >= From && x.DOB.Date <= To);
            }

            int pageSize = 50;
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
        [RequestSizeLimit(230 * 1024 * 1024)]
        public async Task<IActionResult> Create(Artist artist, int[] SelectedRoleId, int[] SelectedHobbyId, int[] SelectedSkillID)
        {
            try
            {
                ViewData["Ethnicity"] = context.Ethnicities.ToList();
                if (ModelState.IsValid)
                {
                    IFormFile postedFile;
                    string artistName = artist.Name.ToString();
                    if (artist.ImageFile != null)
                    {
                        postedFile = artist.ImageFile;
                        string extension = Path.GetExtension(postedFile.FileName);
                        if (FileValid(postedFile, extension))
                        {
                            string imagePath = await UploadFile(artistName, postedFile);
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
                                string imagePath = await UploadFile(artistName, postedFile);
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
            var artist = context.Artists.Include(x => x.ArtistGalleries)
                .Include(x => x.Ethnicity)
                .Include(x => x.Category)
                .Include(x => x.ArtistSkills).ThenInclude(x => x.Skill)
                .Include(x => x.ArtistHobbies).ThenInclude(x => x.Hobby)
                .Include(x => x.ArtistRoles).ThenInclude(x => x.Role).FirstOrDefault(x => x.Id== id);

            ViewBag.Artist = context.Artists.ToList();
            return View(artist);
        }

        //Edit
        public IActionResult Edit(int id)
        {
            var artist = context.Artists.Include(x => x.ArtistRoles).Include(x => x.ArtistHobbies).Include(x => x.ArtistSkills).FirstOrDefault(x => x.Id == id);
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            ViewBag.Category = context.Categories.ToList();
            ViewBag.Role = context.Roles.ToList();
            ViewBag.Hobby = context.Hobbies.ToList();
            ViewBag.Skill = context.Skills.ToList();
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Artist artist, int[] SelectedRoleId, int[] SelectedHobbyId, int[] SelectedSkillId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldArtist = context.Artists.Include(x => x.ArtistRoles).Include(x => x.ArtistHobbies).Include(x => x.ArtistSkills).FirstOrDefault(x => x.Id == id);
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
                                string imagePath = await UploadFile(artistName, postedFile);
                                artist.Dp = imagePath;
                            }
                            else
                            {
                                TempData["message.filevalidation"] = "File not valid";
                                return View(oldArtist);
                            }
                            if (oldArtist.Dp != null)
                            {
                                string oldImagePath = oldArtist.Dp;
                                DeleteRespondingImage(oldImagePath);
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
                                if (!oldArtist.ArtistRoles.Any(x => x.RoleId == selectedRoleId))
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

        private void DeleteImage(string imagePath)
        {
            string imgFullPath = Path.Combine(hostEnvironment.WebRootPath, imagePath);
            if (System.IO.File.Exists(imgFullPath))
            {
                System.IO.File.Delete(imgFullPath);
            }
        }

        private void DeleteRespondingImage(string imagePath)
        {
            imagePath = imagePath.Remove(0, 1);
            //deleting original image
            string originalImgPath = imagePath;
            DeleteImage(imagePath);
            //deleting responding images
            string extension = Path.GetExtension(imagePath);
            string imageName = imagePath.Split(".")[0];

            //deleting thumbnail images
            string thumbImage = imageName + "-thumb" + extension;
            DeleteImage(thumbImage);

            //deleting medium images
            string mediumImage = imageName + "-medium" + extension;
            DeleteImage(mediumImage);

            //deleting thumbnail images
            string fullscreenImage = imageName + "-fullscreen" + extension;
            DeleteImage(fullscreenImage);
        }

        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
                var gallery = artist.ArtistGalleries.Where(x => x.ArtistId == id);
                if (artist.Dp != null)
                {
                    string imagePath = artist.Dp;
                    DeleteRespondingImage(imagePath);
                }
                foreach (var item in gallery)
                {
                    if (item.Gallery != null)
                    {
                        string imagePath = item.Gallery;
                        DeleteRespondingImage(imagePath);
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
        [RequestSizeLimit(230 * 1024 * 1024)]
        public async Task<IActionResult> GalleryUpdate(int id, Artist artist)
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
                        string imagePath = await UploadFile(artistName, postedFile);
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
            //return PartialView("_GalleryUpdate", newArtist);
            // }
        }


        public IActionResult DeleteGallery(int id)
        {
            var artistGallery = context.ArtistGalleries.Find(id);
            var artistId = artistGallery.ArtistId;
            if (artistGallery.Gallery != null)
            {
                string imagePath = artistGallery.Gallery;
                DeleteRespondingImage(imagePath);
              
            }
            context.ArtistGalleries.Remove(artistGallery);
            context.SaveChanges();
            return RedirectToAction(nameof(GalleryIndex), new { id = artistId });
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Contact(int id)
        {
            return View();
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
        /* private string UploadFile(string artistName, IFormFile file)
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
 */

        private async Task<string> UploadFile(string artistName, IFormFile file)
        {
            try
            {
                IFormFile postedFile = file;
                var tasks = new List<Task>();
                tasks.Add(Task.Run(async () =>
                {
                    string imgPath = "/images/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/";

                    string directory = hostEnvironment.WebRootPath + imgPath;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filename = artistName;
                    var extension = Path.GetExtension(file.FileName);
                    filename = filename + " " + Guid.NewGuid();
                    filename = SlugHelper.GenerateSlug(filename);
                    var imgfullpath = filename + extension;

                    //Saving Original File with fileStream for better performance becasue original size is very high
                    string path = Path.Combine(directory, imgfullpath);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        postedFile.CopyTo(fileStream);
                    }
                    filename = imgPath + filename;
                    image = filename + extension;
                    using var imageResult = await Image.LoadAsync(file.OpenReadStream());
                    await SaveImage(imageResult, $"{filename}" + "-fullscreen" + $"{ extension}", FullScreenWidth);
                    await SaveImage(imageResult, $"{filename}" + "-medium" + $"{ extension}", MediumWidth);
                    await SaveImage(imageResult, $"{filename}" + "-thumb" + $"{ extension}", ThumbnailWidth);
                }));
                await Task.WhenAll(tasks);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return image;

        }

        private async Task SaveImage(Image image, string name, int resizeWidth)
        {
            var rootPath = hostEnvironment.WebRootPath;
            var width = image.Width;
            var height = image.Height;
            if (width > resizeWidth)
            {
                height = (int)((double)resizeWidth / width * height);
                width = resizeWidth;
            }

            image.Mutate(x => x
            .Resize(new Size(width, height)));
            await image.SaveAsJpegAsync(rootPath + name, new JpegEncoder
            {
                Quality = 100
            });

        }

    }
}
