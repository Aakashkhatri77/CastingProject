using CastingProject.Data;
using CastingProject.Helper;
using CastingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace CastingProject.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png",".JPG"};
        private string image;
        private const int MediumWidth = 400;
        private const int FullScreenWidth = 1400;

        public BlogsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            this.hostEnvironment = hostEnvironment;

        }

        // GET: BlogsController
        public ActionResult Index()
        {
            var blogs = context.Blogs.ToList();
            return View(blogs);
        }

        // GET: BlogsController/Details/5
        public ActionResult Details(int id)
        {
            var blog = context.Blogs.Find(id);
            return View(blog);
        }

        // GET: BlogsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blogs blogs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IFormFile postedFile; ;
                    string blogsTitle = blogs.Title.ToString();
                    if (blogs.ImageFile != null)
                    {
                        postedFile = blogs.ImageFile;
                        string extension = Path.GetExtension(postedFile.FileName);
                        if (FileValid(postedFile, extension))
                        {
                            string imagePath = await UploadFile(blogsTitle, postedFile);
                            blogs.Picture = imagePath;
                        }
                        else
                        {
                            TempData["message.filevalidation"] = "File not valid";
                            return View();
                        }
                        blogs.Slug = SlugHelper.GenerateSlug(blogs.Title) + " " + SlugHelper.GenerateSlug(blogs.HighlightLine);
                        await context.Blogs.AddAsync(blogs);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                return View();
            }
                return RedirectToAction(nameof(Index));
        }

        // GET: BlogsController/Edit/5
        public ActionResult Edit(int id)
        {
            var blog = context.Blogs.Find(id);
            return View(blog);
        }

        // POST: BlogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blogs blogs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldBlog = context.Blogs.Find(id);
                    IFormFile postedFile;
                    string blogsTitle = blogs.Title.ToString();

                    if (oldBlog != null)
                    {
                        if (blogs.ImageFile != null)
                        {
                            postedFile = blogs.ImageFile;
                            string extension = Path.GetExtension(postedFile.FileName);
                            if (FileValid(postedFile, extension))
                            {
                                string imagePath = await UploadFile(blogsTitle, postedFile);
                                blogs.Picture = imagePath;
                            }
                            else
                            {
                                TempData["message.filevalidation"] = "File not valid";
                                return View(oldBlog);
                            }

                            if (oldBlog.Picture != null)
                            {
                                string oldImagePath = oldBlog.Picture;
                                DeleteRespondingImage(oldImagePath);
                            }
                            oldBlog.Picture = blogs.Picture;
                        }

                        oldBlog.Title = blogs.Title;
                        oldBlog.Content = blogs.Content;
                        oldBlog.HighlightLine = blogs.HighlightLine;
                        oldBlog.PostStatus = blogs.PostStatus;
                        oldBlog.PublishDate = blogs.PublishDate;
                        oldBlog.Slug = blogs.Slug;
                    }
                }
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

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

        // GET: BlogsController/Delete/5
       /* public ActionResult Delete(int id)
        {
            return View();
        }*/

        // POST: BlogsController/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var blog = context.Blogs.Find(id);
                if (blog.Picture != null)
                {
                    string imagePath = blog.Picture;
                    DeleteRespondingImage(imagePath);
                }
                context.Blogs.Remove(blog);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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


        private async Task<string> UploadFile(string blogsTitle, IFormFile file)
        {
            try
            {
                IFormFile postedFile = file;
                var tasks = new List<Task>();
                tasks.Add(Task.Run(async () =>
                {
                    string imgPath = "/blogs/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/";

                    string directory = hostEnvironment.WebRootPath + imgPath;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string filename = blogsTitle;
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
