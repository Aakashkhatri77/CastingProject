using Azure.Core;
using System.Web;


namespace CastingProject.Helper
{
    public class ImageSize
    {
        public static string Get(string url, string resize)
        {
       

            if (url != null)
            {
                string imgPath = url.Split('.')[0];
                string ext = url.Split('.')[1];


                switch (resize)
                {
                    case "thumb":
                        return "https://localhost:7043" + imgPath + "-thumb." + ext;
                    case "medium":
                        return "https://localhost:7043" + imgPath + "-medium." + ext;
                    case "fullscreen":
                        return "https://localhost:7043" + imgPath + "-fullscreen." + ext;
                    default:
                        return "https://localhost:7043" + imgPath + ext;

                }
            }
            else
            {
                return "";
            }
        }
    }
}
