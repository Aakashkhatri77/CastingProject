using CastingProject.Data;
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

        public IActionResult Index(Artist filter, string Ethnicity, string height, string searchText, int? page)
        {
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            var query = context.Artists.Include(x => x.Ethnicity).AsQueryable();
            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(s => s.Name.ToLower().Contains(searchText.ToLower()));
                ViewData["CurrentFilter"] = searchText;
            }
            if (filter.Gender is not null)
            {
                query = query.Where(x => x.Gender == filter.Gender);
            }
            if (filter.Skin_Color is not null)
            {
                query = query.Where(x => x.Skin_Color == filter.Skin_Color);
            }
            if (Ethnicity is not null)
            {
                query = query.Where(x =>x.Ethnicity.Name == Ethnicity);
            }
            if (height is not null)
            {
                if (height == "3-4")
                {
                    query = query.Where(x => x.Height >= 3 && x.Height <= 4);
                }
                else if (height == "4-5")
                {
                    query = query.Where(x => x.Height >= 4 && x.Height <= 5);
                }
                else if (height == "5-6")
                {
                    query = query.Where(x => x.Height >= 5 && x.Height <= 6);
                }
                else if (height == "6-7")
                {
                    query = query.Where(x => x.Height >= 6 && x.Height <= 7);
                }
                else if (height == "7-8")
                {
                    query = query.Where(x => x.Height >= 7 && x.Height <= 8);
                }
            }
            int pageSize = 4;
            return View(PaginatedList<Artist>.CreateAsync(query.AsNoTracking(), page ?? 1, pageSize));

            //ViewBag.Artist = query.ToList();

         /* 
            if (ModelState.IsValid && filterResult.Ethnicity != null)
            {


                var result = from s in context.Artists select s;
                result = result.Where(s => s.Gender == filterResult.Gender);

                result = result.Where(
                    s => s.Gender == filterResult.Gender
                    && s.EthnicityId == filterResult.EthnicityId
                    || s.Skin_Color == filterResult.Skin_Color
                    || s.Height.Contains(filterResult.Height));


                ViewBag.Artist = result;
                return View();
            }
            else
            {
                var artist = context.Artists.ToList();
                var ethnicity = context.Ethnicities.ToList();
                ViewBag.Ethnicity = context.Ethnicities.ToList();
                ViewBag.Artist = artist;
            return View();
            }*/
        }

        public IActionResult Create()
        {
            var artist = new Artist();
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            return View(artist);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Artist artist)
        {
            try
            {
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
            var artist = context.Artists.Include(x => x.ArtistGalleries).FirstOrDefault(x => x.Id == id);
            return View(artist);
        }

        //Edit
        public IActionResult Edit(int id)
        {
            var artist = context.Artists.Find(id);
            ViewBag.Ethnicity = context.Ethnicities.ToList();
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Artist artist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldArtist = context.Artists.Find(id);
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
                        oldArtist.Name = artist.Name;
                        oldArtist.Gender = artist.Gender;
                        oldArtist.EthnicityId = artist.EthnicityId;
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

        public string EnumNames()
        {
            var enums = "";
            foreach (var item in Enum.GetNames(typeof(EnumField.Gender)))
            {
                enums = item.ToString();
            }
            return enums;
        }

    }


}
