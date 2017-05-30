using Azure_Blob_Demo.Models;
using Azure_Blob_Demo.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Azure_Blob_Demo.Controllers
{
    public class HomeController : Controller
    {

        BlobUtility utility;
        ApplicationDbContext db;


        string accountName = "yourazurestorageaccountname";
        string accountKey = "yourazurestorageaccountkey";

        public HomeController()
        {
            utility = new BlobUtility(accountName, accountKey);
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Azure Blob Demo";
            return View();
        }

        [Authorize]
        public ActionResult ImageUpload()
        {
            
            ViewBag.Message = "Image Upload Demo";
            string loggedInUserId = User.Identity.GetUserId();
            List<Image> userImages = (from r in db.Images where r.UserId == loggedInUserId select r).ToList();
            ViewBag.PhotoCount = userImages.Count;
            return View(userImages);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //Deletes the image from your blob container
        public ActionResult DeleteImage(string id)
        {
            
            Image userImage = db.Images.Find(id);
            db.Images.Remove(userImage);
            db.SaveChanges();
            string BlobNameToDelete = userImage.ImageUrl.Split('/').Last();

            //replace "container" with the name of your Blob container
            utility.DeleteBlob(BlobNameToDelete, "container");
            return RedirectToAction("ImageUpload");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                //replace "container" with the name of your Blob container
                string ContainerName = "container"; //hardcoded container name. 
                file = file ?? Request.Files["file"];
                string fileName = Path.GetFileName(file.FileName);
                Stream imageStream = file.InputStream;
                var result = utility.UploadBlob(fileName, ContainerName, imageStream);
                if (result != null)
                {
                    string loggedInUserId = User.Identity.GetUserId();
                    Image userimage = new Image();
                    userimage.Id = new Random().Next().ToString();
                    userimage.UserId = loggedInUserId;
                    userimage.ImageUrl = result.Uri.ToString();
                    db.Images.Add(userimage);
                    db.SaveChanges();
                    return RedirectToAction("ImageUpload");
                }
                else
                {
                    return RedirectToAction("ImageUpload");
                }
            }
            else
            {
                return RedirectToAction("ImageUpload");
            }


        }
    }
}