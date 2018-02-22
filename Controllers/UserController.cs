using qiushi.Entity;
using qiushi.Model;
using qiushi.Task;
using qiushi.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace qiushi.Web.Controllers
{
   
    public class UserController : BaseController
    {
        // GET: User
        [MyAuthen]
        public async Task<ActionResult> Index()
        {
            //GetUserImage(CurrentUser.Id);
            var key = Constant.UserCache + "." + CurrentUser.Id.ToString();
            var score = HttpContext.Cache.Get(key);
            if (score == null)
            {
                var userBl = new UserModel();
                CurrentUser.Score = await userBl.GetScore(CurrentUser);
                HttpContext.Cache.Add(key, CurrentUser.Score, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), CacheItemPriority.Normal, null);
            }
            else
            {
                CurrentUser.Score = int.Parse (score.ToString());
            }
           
           
            return View(CurrentUser);
        }

        [MyAuthen]
        [HttpPost]
        public async Task<ContentResult> UploadFile(HttpPostedFileBase file)
        {
            
            PhotoTask task = new PhotoTask();

            var path = System.Configuration.ConfigurationManager.AppSettings["UserImageFolder"].ToString();
            var siteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString();

            var newFileName = CurrentUser.Id + ".jpg";//file.FileName.Substring(file.FileName.LastIndexOf("."));

            var url = await task.UploadHeadPhoto(path + newFileName, file.InputStream);


            CurrentUser.RemoveUserImageCache();

            return Content(newFileName);
        }

        [MyAuthen]
        public ActionResult Password()
        {
            //GetUserImage(CurrentUser.Id);
            return View(CurrentUser);
        }

        [MyAuthen]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(FormCollection form)
        {
            var password = form["oldPassword"];
            var newPassword = form["newPassword"];
            var repeatPassword = form["repeatPassword"];

            EqualValidation(newPassword, repeatPassword, "确认密码不一致");

            if (MessageEntities.Count() > 0)
            {
                return View("Password", CurrentUser);
            }


            var userBl = new UserModel();
            var user = await userBl.ChangePassword(CurrentUser, password, newPassword);

            AddMessage(user.Message);

            if (user.Message.MessageType == Constant.MessageType.Error)
            {
                
                return View("Password", user);
            }

            //GetUserImage(CurrentUser.Id);
            return View("Password", user);
        }

        //[HttpPost]
        //public async Task<ContentResult> Edit(FormCollection form)
        //{
        //    var email = form["email"];

        //}

        public async Task<ActionResult> GetUser(int id, int page)
        {
            var userBl = new UserModel();
            var user = userBl.GetUser(id);

            var key = Constant.UserCache + "." + user.Id.ToString();
            var score = HttpContext.Cache.Get(key);
            if (score == null)
            {

                user.Score = await userBl.GetScore(user);
                HttpContext.Cache.Add(key, user.Score, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0), CacheItemPriority.Normal, null);
            }
            else
            {
                user.Score = int.Parse(score.ToString());
            }

            ViewBag.page = page;

            return View("Detail", user);
        }

        [MyAuthen]
        public async Task<ActionResult> List(int page)
        {
            var userBl = new UserModel();
            var user = await userBl.List( page, Constant.numArticlePerPage);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = user.Item2;

            return View(user.Item1);
        }

        [MyAuthen(roleName:"admin")]
        public async Task<ActionResult> Disable(int id, int page)
        {
            var userBl = new UserModel();
            var user = await userBl.Enable(id, false);

            return RedirectToAction("List", new { page = 1 });
        }

        [MyAuthen(roleName: "admin")]
        public async Task<ActionResult> Enable(int id, int page)
        {
            var userBl = new UserModel();
            var user = await userBl.Enable(id, true);

            return RedirectToAction("List", new { page = 1 });
        }
    }
}